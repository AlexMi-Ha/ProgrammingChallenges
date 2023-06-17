
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class ArithmeticInstruction : Node {

        public int Argument { get; private init; }
        public TokenType Type { get; private set; }

        public ArithmeticInstruction(TokenType type, int arg) {
            Type = type;
            Argument = arg;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }

        public override string ToString() {
            return $"{Type} {Argument}";
        }
    }
}
