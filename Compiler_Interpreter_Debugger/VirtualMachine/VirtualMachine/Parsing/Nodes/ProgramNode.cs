
using VirtualMachine.Interpreter.Abstract;

namespace VirtualMachine.Parsing.Nodes {
    internal class ProgramNode : Node {

        public List<Node> Instructions { get; private set; }

        public ProgramNode() {
            Instructions = new List<Node>();
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
