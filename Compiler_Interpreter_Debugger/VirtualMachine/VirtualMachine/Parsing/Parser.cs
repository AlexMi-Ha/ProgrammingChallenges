using VirtualMachine.Lexing;
using VirtualMachine.Lexing.Tokens;
using VirtualMachine.Parsing.Nodes;
using VirtualMachine.Parsing.Nodes.Instructions;

namespace VirtualMachine.Parsing {
    internal class Parser {

        private Lexer _lexer;

        public Parser(Lexer input) {
            _lexer = input;
        }

        public ProgramNode Parse() {
            var prog = new ProgramNode();
            var instructionCount = 0;
            while(_lexer.LookAhead.Type != TokenType.EOF) {
                var instr = ParseInstruction();
                if(instr is LabelInstruction label) {
                    var succ = prog.LabelAdresses.TryAdd(label.Label, instructionCount);
                    if(!succ) {
                        throw new Exception($"Label {label.Label} already defined");
                    }
                }
                prog.Instructions.Add(instr);
                instructionCount++;
            }
            return prog;
        }

        public Node ParseInstruction() {
            switch (_lexer.LookAhead.Type) {
                case TokenType.ADD:
                case TokenType.ADDC:
                case TokenType.SUB:
                case TokenType.SUBC:
                case TokenType.MUL:
                case TokenType.MULC:
                case TokenType.DIV:
                case TokenType.DIVC:
                case TokenType.MOD:
                case TokenType.MODC:
                    return ParseOperatorInstruction();

                case TokenType.CMP:
                case TokenType.CMPC:
                case TokenType.AND:
                case TokenType.ANDC:
                case TokenType.OR:
                case TokenType.ORC:
                case TokenType.XOR:
                case TokenType.XORC:
                case TokenType.NOT:
                case TokenType.SHIFTL:
                case TokenType.SHIFTLC:
                case TokenType.SHIFTR:
                case TokenType.SHIFTRC:
                    return ParseLogicInstruction();

                case TokenType.LOAD:
                case TokenType.LOADC:
                case TokenType.STORE:
                    return ParseMemoryInstruction();

                case TokenType.JMP:
                case TokenType.JGT:
                case TokenType.JGE:
                case TokenType.JLT:
                case TokenType.JLE:
                case TokenType.JEQ:
                case TokenType.JNE:
                case TokenType.CALL:
                case TokenType.RETURN:
                    return ParseJumpInstruction();

                case TokenType.LABEL:
                    return ParseLabelInstruction();

                case TokenType.HOLD:
                    return new HoldInstruction();

                case TokenType.IN:
                case TokenType.OUT:
                case TokenType.OUTCHAR:
                    return ParseIOInstruction();

                case TokenType.PUSH:
                case TokenType.POP:
                    return ParseStackInstruction();
            }
            throw _lexer.CreateCompilerException("Unexpected input", "");
        }

        private Node ParseOperatorInstruction() {
            var op = _lexer.Advance();
            var arg = _lexer.Expect(TokenType.NUMBER);
            return new ArithmeticInstruction(op.Type, int.Parse(arg.Value!));
        }

        private Node ParseLogicInstruction() {
            var op = _lexer.Advance();
            if(op.Type == TokenType.NOT)
                return new LogicInstruction(op.Type, 0);
            var arg = _lexer.Expect(TokenType.NUMBER);
            return new LogicInstruction(op.Type, int.Parse(arg.Value!));
        }

        private Node ParseMemoryInstruction() {
            var op = _lexer.Advance();
            var arg = _lexer.Expect(TokenType.NUMBER);
            return new MemoryInstruction(op.Type, int.Parse(arg.Value!));
        }

        private Node ParseJumpInstruction() {
            var op = _lexer.Advance();
            if (op.Type is TokenType.RETURN)
                return new JumpInstruction(op.Type, null);

            var arg = _lexer.Expect(TokenType.LABEL);
            return new JumpInstruction(op.Type, arg.Value!);
        }

        private Node ParseLabelInstruction() {
            var label = _lexer.Expect(TokenType.LABEL);
            _lexer.Expect(TokenType.DOUBLECOLON);

            return new LabelInstruction(label.Value!);
        }

        private Node ParseIOInstruction() {
            var op = _lexer.Advance();
            return new IOInstruction(op.Type);
        }

        private Node ParseStackInstruction() {
            var op = _lexer.Advance();
            return new StackInstruction(op.Type);
        }
    }
}
