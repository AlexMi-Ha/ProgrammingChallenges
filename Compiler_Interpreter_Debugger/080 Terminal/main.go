package main

import (
	"bufio"
	"errors"
	"fmt"
	"io"
	"os"
	"os/exec"
	"os/signal"
	"path/filepath"
	"strings"
)

var (
	process    *os.Process
	workingDir string = "/"
)

func main() {
	handleSignals()
	workingDir, _ = os.Getwd()
	scanner := bufio.NewReader(os.Stdin)
	for {
		fmt.Print(workingDir + " $ ")
		input, errs := scanner.ReadString('\n')
		if errs != nil {
			fmt.Println()
			continue
		}
		if strings.TrimSpace(input) == "" {
			fmt.Println()
			continue
		}
		if strings.TrimSpace(strings.ToLower(input)) == "exit" {
			return
		}
		cmd, err := parseInput(strings.TrimSpace(input))
		if err != nil {
			fmt.Printf("Terminal: %v\n", err.Error())
			continue
		}

		err = executeProgram(cmd)
		process = nil
		if err != nil {
			fmt.Printf("Terminal: %v\n", err.Error())
			continue
		}
	}
}

func executeProgram(command Command) (err error) {
	if command.ProgramPath == "cd" {
		return executeCd(command)
	}
	if command.PipedTo != nil {
		return executePipedProgram(command)
	}

	c1 := exec.Command(command.ProgramPath, command.Args...)
	c1.Stdin = os.Stdin
	c1.Stdout = os.Stdout
	process = c1.Process
	if err = c1.Start(); err != nil {
		return
	}
	if err = c1.Wait(); err != nil {
		return
	}
	return nil
}

func executePipedProgram(command Command) (err error) {
	c1 := exec.Command(command.ProgramPath, command.Args...)
	c2 := exec.Command(command.PipedTo.ProgramPath, command.PipedTo.Args...)

	r, w := io.Pipe()
	c1.Stdout = w
	c2.Stdin = r

	c2.Stdout = os.Stdout

	if err = c1.Start(); err != nil {
		return
	}
	process = c1.Process
	if err = c2.Start(); err != nil {
		return
	}
	if err = c1.Wait(); err != nil {
		return
	}
	if err = w.Close(); err != nil {
		return
	}
	if err = c2.Wait(); err != nil {
		return
	}
	return err
}

func executeCd(command Command) error {
	arg := strings.Join(command.Args, " ")
	resolvedPath := filepath.Join(workingDir, arg)

	_, err := os.Stat(resolvedPath)
	if err != nil {
		return err
	}
	workingDir = resolvedPath
	return nil
}

func parseInput(input string) (commandRet Command, errRet error) {
	pipeArr := strings.Split(input, "|")
	if len(pipeArr) == 2 {
		cmd1, err1 := parseInput(pipeArr[0])
		cmd2, err2 := parseInput(pipeArr[1])

		if err1 != nil {
			return commandRet, err1
		}
		if err2 != nil {
			return commandRet, err2
		}

		cmd1.PipedTo = &cmd2
		commandRet = cmd1
		return
	}
	if len(pipeArr) == 1 {
		cmdArr := strings.Split(strings.Trim(pipeArr[0], " "), " ")
		progDir, err := searchProgramDir(cmdArr[0])
		if err != nil {
			errRet = err
			return
		}
		return Command{progDir, cmdArr[1:], nil}, nil
	}
	errRet = errors.New("Cannot pipe multiple times")
	return
}

func searchProgramDir(progName string) (string, error) {
	if progName == "cd" {
		return "cd", nil
	}
	_PATH := os.Getenv("PATH")
	allPaths := strings.Split(_PATH, ";")
	for _, path := range allPaths {
		if path == "" {
			continue
		}
		entries, err := os.ReadDir(path)

		if err != nil {
			continue
		}

		for _, file := range entries {
			if !file.IsDir() && strings.ToLower(strings.Split(file.Name(), ".")[0]) == strings.ToLower(progName) {
				return filepath.Join(path, file.Name()), nil
			}
		}
	}
	return "", errors.New("Cannot find program '" + progName + "' in $PATH")
}

func handleSignals() {
	signalChannel := make(chan os.Signal, 2)
	signal.Notify(signalChannel, os.Interrupt)
	go func() {
		for {
			sig := <-signalChannel
			//handle SIGINT
			if process != nil {
				process.Signal(sig)
			}
		}
	}()
}

type Command struct {
	ProgramPath string
	Args        []string
	PipedTo     *Command
}
