using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class IOInstruction : Node {

        public TokenType Type { get; private set; }

        public IOInstruction(TokenType type) {
            Type = type;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }

        public override string ToString() {
            return $"{Type}";
        }
    }
}
