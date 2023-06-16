
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class StackInstruction : Node {

        public TokenType Type { get; private set; }

        public StackInstruction(TokenType type) {
            Type = type;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
