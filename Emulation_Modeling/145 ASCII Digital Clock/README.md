# Ascii Digital Clock

This is actually my first program I made in Go as I'm currently learning it as a new language - So don't expect the code to be too clean!

The heart here is the definition of the ascii building blocks
```go
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
```
which then get put together accordingly.

## Challenges
One little challenge I faced (ignoring writing this in a completely new language) was how I manage to clear the console after a number was displayed.

I wanted to have the digital numbers like
```
      _     _   _
   |   | .  _| |_
   |   | . |_  |_|
```
with the dot pulsating to give the seconds.

But I didn't know how to clear the last 3 lines of the console.

I decided to use the escape chars
```go
func clearLines(lineCount int) {
	s := "\033[1A" // Move up a line
	s += "\033[K"  // Clear the line
	s += "\r"      // Move back to the beginning of the line
	s = strings.Repeat(s, lineCount)
	fmt.Print(s)
}
```
But these do not work in every terminal - Well that's a skill issue on the users part ^^