package main

import (
	"fmt"
	"math"
	"strings"
	"time"
)

func main() {

	var displaySecondDot = true

	for {
		currentTime := time.Now()

		digitsHour := addStringArrays(getNumberAsAscii(currentTime.Hour()/10), getNumberAsAscii(currentTime.Hour()%10), " ")
		digitsMinute := addStringArrays(getNumberAsAscii(currentTime.Minute()/10), getNumberAsAscii(currentTime.Minute()%10), " ")

		secondDot := " . "
		if !displaySecondDot {
			secondDot = "   "
		}

		fmt.Println(digitsHour[0] + space + digitsMinute[0])
		fmt.Println(digitsHour[1] + secondDot + digitsMinute[1])
		fmt.Println(digitsHour[2] + secondDot + digitsMinute[2])

		displaySecondDot = !displaySecondDot

		sleepTime := math.Max(float64(currentTime.UnixMilli()+500-time.Now().UnixMilli()), 0)
		time.Sleep(time.Duration(sleepTime) * time.Millisecond)

		clearLines(3)
	}
}

func clearLines(lineCount int) {
	s := "\033[1A" // Move up a line
	s += "\033[K"  // Clear the line
	s += "\r"      // Move back to the beginning of the line
	s = strings.Repeat(s, lineCount)
	fmt.Print(s)
}

const (
	right         = "  |"
	left          = "|  "
	rightLeft     = "| |"
	up            = " _ "
	rightLeftDown = "|_|"
	rightDown     = " _|"
	leftDown      = "|_ "
	dot           = " . "
	space         = "   "
)

func getNumberAsAscii(a int) []string {
	var ret [3]string
	a %= 10
	switch a {
	case 1:
		ret[0] = space
		ret[1] = right
		ret[2] = right
	case 2:
		ret[0] = up
		ret[1] = rightDown
		ret[2] = leftDown
	case 3:
		ret[0] = up
		ret[1] = rightDown
		ret[2] = rightDown
	case 4:
		ret[0] = space
		ret[1] = rightLeftDown
		ret[2] = right
	case 5:
		ret[0] = up
		ret[1] = leftDown
		ret[2] = rightDown
	case 6:
		ret[0] = up
		ret[1] = leftDown
		ret[2] = rightLeftDown
	case 7:
		ret[0] = up
		ret[1] = right
		ret[2] = right
	case 8:
		ret[0] = up
		ret[1] = rightLeftDown
		ret[2] = rightLeftDown
	case 9:
		ret[0] = up
		ret[1] = rightLeftDown
		ret[2] = rightDown
	default:
		ret[0] = up
		ret[1] = rightLeft
		ret[2] = rightLeftDown
	}
	return ret[:]
}

func addStringArrays(arr1, arr2 []string, space string) []string {
	if len(arr1) != len(arr2) {
		panic("Arrays are not equal in size")
	}
	for i := 0; i < len(arr1); i++ {
		arr1[i] += space + arr2[i]
	}
	return arr1
}
