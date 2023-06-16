
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexer.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class ArithmeticInstruction : Node {

        public LiteralNode Argument { get; private init; }
        public TokenType Type { get; private set; }

        public ArithmeticInstruction(TokenType type, LiteralNode arg) {
            Type = type;
            Argument = arg;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
