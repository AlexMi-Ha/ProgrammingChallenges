# Virtual Machine / Assembly Interpreter

Here I wrote a small Lexer - Parser - Interpreter Programm for my own little assembly implementation.

It has a Accumulator, a ValueStack, a CallStack, 1024 memory cells with each 32 bit, a NegativeFlag and a SIG_HOLD flag

## Example Programms

<details>
<summary>Hello World</summary>
```asm
LOADC 72
    OUTCHAR
    LOADC 101
    OUTCHAR
    LOADC 108
    OUTCHAR
    OUTCHAR
    LOADC 111
    OUTCHAR
    LOADC 32
    OUTCHAR
    LOADC 87
    OUTCHAR
    LOADC 111
    OUTCHAR
    LOADC 114
    OUTCHAR
    LOADC 108
    OUTCHAR
    LOADC 100
    OUTCHAR
    LOADC 33
    OUTCHAR
```
</details>

<details>
<summary>Square Root</summary>
```asm
    entry:
        IN
        JGT goodInput
        HOLD

    goodInput:
        STORE 0 
        STORE 1
        LOADC 0
        STORE 2
        JMP loop

    loop:
        LOAD 0
        DIVC 2
        CMP 2
        JLT end

        LOAD 0
        DIV 1
        ADD 1
        DIVC 2
        STORE 1

        LOAD 2
        ADDC 1
        STORE 2
        JMP loop
        
    end:
        LOAD 1
        OUT
        JMP entry
```
</details>

<details>
<summary>Factorial</summary>
```asm
    entry:
        IN
        JGT goodInput
        HOLD

    goodInput:
        STORE 0
        STORE 1
        JMP loop

    loop:
        LOAD 0
        CMPC 1
        JLE end

        LOAD 0
        SUBC 1
        STORE 0

        MUL 1
        STORE 1
        JMP loop

    end:
        LOAD 1
        OUT
        JMP entry
```
</details>

## Instruction set

### Arithmetic Operations
| Instruction | Description |
| - | - |
| ADD *adress* | Add the value at the address to the Akku |
| ADDC *number* | Add a constant to the Akku |
| SUB *adress* | Subtract the value at the address from the Akku  |
| SUBC *number* | Subtract a constant from the Akku |
| MUL *adress* | Multiply the value at the address to the Akku  |
| MULC *number* | Multiply a constant to the Akku |
| DIV *adress* | Divide the Akku by the value at the address |
| DIVC *number* | Divide the Akku by a constant |
| MOD *adress* | Modulo the Akku by the value at the address |
| MODC *number* | Modulo the Akku by a constant |

### Logic Operations
| Instruction | Description |
| - | - |
| CMP *adress* | Compare the value at the address to the Akku  |
| CMPC *number* | Compare a constant to the Akku |
| AND *adress* | Bitwise And with the value at the address and the Akku  |
| ANDC *number* | Bitwise And with a constant and the Akku |
| OR *adress* | Bitwise OR with the value at the address and the Akku  |
| ORC *number* | Bitwise OR with a constant and the Akku |
| XOR *adress* | Bitwise XOR with the value at the address and the Akku  |
| XORC *number* | Bitwise XOR with a constant and the Akku |
| NOT | Bitwise NOT the Akku  |
| SHIFTL *adress* | Shift the Akku left by the value at the address  |
| SHIFTLC *number* | Shift the Akku left by a constant |
| SHIFTR *adress* | Shift the Akku right by the value at the address  |
| SHIFTRC *number* | Shift the Akku right by a constant |

### Memory Operations
| Instruction | Description |
| - | - |
| LOAD *adress* | Load the value at the address into the Akku  |
| LOADC *number* | Load the constant into the Akku |
| STORE *adress* | Store the Akku at the address  |

### Jumps
| Instruction | Description |
| - | - |
| JMP *label* | Jump to the label |
| JGT *label* | Jump to the label if Akku > 0 |  
| JGE *label* | Jump to the label if Akku >= 0 |  
| JLT *label* | Jump to the label if Akku < 0 |  
| JLE *label* | Jump to the label if Akku <= 0 |  
| JEQ *label* | Jump to the label if Akku == 0 |  
| JNE *label* | Jump to the label if Akku != 0 |

### Misc
| Instruction | Description |
| - | - |
| HOLD | Stop the program |
| IN | Read input from stdin into the Akku |
| OUT | Write the Akku (as a number) to stdout |
| OUTCHAR | Write the Akku (as a char) to stdout |

### Functions / Stack manipulation
| Instruction | Description |
| - | - |
| CALL *label* | Push the current address to the Call Stack and jump to the label |
| RETURN | Pop the Call Stack and jump to the address |
| PUSH | Push the Akku to the Value Stack |
| POP | Pop the Value Stack to the Akku |
