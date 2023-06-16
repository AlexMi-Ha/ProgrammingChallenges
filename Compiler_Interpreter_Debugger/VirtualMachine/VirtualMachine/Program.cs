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

var parser = new Parser(LexerFactory.CreateLexer(assemblyFactorial));
var result = parser.Parse();
result.AcceptVisitor(new InterpreterVisitor());

Console.ReadLine();