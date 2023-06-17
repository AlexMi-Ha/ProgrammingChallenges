using VirtualMachine.Interpreter;
using VirtualMachine.Lexing;
using VirtualMachine.Parsing;

string assemblyHelloWorld = """

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
""";

string assemblySqrt = """
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
""";

string assemblyFactorial = """
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

""";


string assemblyFizzbuzz = """
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
""";

var parser = new Parser(LexerFactory.CreateLexer(assemblyFizzbuzz));
var result = parser.Parse();
result.AcceptVisitor(new InterpreterVisitor());

Console.ReadLine();