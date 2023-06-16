namespace VirtualMachine.Lexing.Tokens {
    internal enum TokenType {
        EOF,
        NUMBER,

        // operators
        ADD,
        ADDC,

        SUB,
        SUBC,

        MUL,
        MULC,
        
        DIV,
        DIVC,

        MOD,
        MODC,
        
        // logic
        CMP,
        CMPC,

        AND,
        ANDC,
        OR,
        ORC,
        XOR,
        XORC,
        NOT,
        NOTC,

        SHIFTL,
        SHIFTLC,
        SHIFTR, 
        SHIFTRC,
        
        // memory
        LOAD,
        LOADC,
        STORE,

        // jump
        JMP,
        JGT,
        JGE,
        JLT,
        JLE,
        JEQ,
        JNE,
        LABEL,

        // misc
        HOLD,
        IN,
        OUT,
        OUTCHAR,

        // stack
        CALL,
        RETURN,
        PUSH,
        POP,

        WHITESPACE,
    }
}
