# 044 FizzBuzz

Fizzbuzz is a popular algortihm in coding interviews and other challenges, so it was not unknown to me!

## FizzBuzz in Python
First I did the Python implementation, which one of the default solutions:
- Checking mod 3: Adding Fizz to string
- Checking mod 5: Adding Buzz to string
- If neither got added: add i to string
- Print string

## FizzBuzz in Assembly
Here i used the Assembly Interpreter I've written for challenge [078 Virtual Machine](../../Compiler_Interpreter_Debugger/078%20VirtualMachine/README.md).

The pseudocode for the program looks something like this:
```pseudo
PUSH 0 to stack
CHECK mod3:
    PRINT Fizz
    POP stack
    ADD 1
    PUSH to stack again

CHECK mod5:
    PRINT Buzz
    POP stack
    ADD 1
    PUSH to stack again

count = POP stack
CHECK count == 0 (Fizz or buzz was NOT executed):
    PRINT number

PRINT line feed

repeat
```

Here I've used the stack as a counter/flag, similar to how I used the string length in the python example.

For the specific implementation, look at the [Assembly file](./Fizzbuzz.asm)