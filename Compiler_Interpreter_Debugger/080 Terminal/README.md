# Terminal Shell

I used this Programming Challenge to learn more about the language `go` sooo don't expect clean code :p

This Shell doesn't use anything special but basic operations. It has these phases:
- Wait for input
- Parse input (Check for '|', seperate arguments)
- Search the program in the `$PATH` directories
- Execute the program (and maybe pipe its output to a second program)
- Wait for the program
- Repeat

## Working Directory
Using the command 'cd' you can change your working directory

## Piping
By writing a `|` symbol you can pipe the output of a program into another program (e.g. `cat main.go | grep func` -> Search all 'func' in main.go)

## Signals
When pressing Ctrl+C, `SIGINT` ist forwarded to the running process