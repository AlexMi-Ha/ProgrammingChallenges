using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class MemoryInstruction : Node {

        public int Argument { get; private init; }
        public TokenType Type { get; private set; }

        public MemoryInstruction(TokenType type, int arg) {
            this.Type = type;
            Argument = arg;
        }


        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
