entry:
    IN
    JGT goodInput
    HOLD

goodInput:
    STORE 0
    LOADC 0
    STORE 1
    JMP loop

loop:
    LOAD 1
    CMP 0
    JGT end
    LOADC 0
    PUSH

    LOAD 1
    MODC 3
    JNE notMod3
    CALL fizz
    POP
    ADDC 1
    PUSH

notMod3:
    LOAD 1
    MODC 5
    JNE notMod5
    CALL buzz
    POP
    ADDC 1
    PUSH

notMod5:
    POP
    JGT iter
    LOAD 1
    CALL printNumberInAkku

iter:
    CALL printLineFeed
    LOAD 1
    ADDC 1
    STORE 1
    JMP loop

end:
    JMP entry


fizz:
    PUSH
    LOADC 70
    OUTCHAR
    LOADC 105
    OUTCHAR
    LOADC 122
    OUTCHAR
    OUTCHAR
    POP
    RETURN

buzz:
    PUSH
    LOADC 66
    OUTCHAR
    LOADC 117
    OUTCHAR
    LOADC 122
    OUTCHAR
    OUTCHAR
    POP
    RETURN

printNumberInAkku:
    OUT
    RETURN

printLineFeed:
    PUSH
    LOADC 10
    OUTCHAR
    POP
    RETURN