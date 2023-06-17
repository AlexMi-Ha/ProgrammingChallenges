
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class JumpInstruction : Node {

        public string? Argument { get; private init; }
        public TokenType Type { get; private set; }

        public JumpInstruction(TokenType type, string? arg) {
            Type = type;
            Argument = arg;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }

        public override string ToString() {
            return $"{Type} {Argument ?? ""}";
        }
    }
}
