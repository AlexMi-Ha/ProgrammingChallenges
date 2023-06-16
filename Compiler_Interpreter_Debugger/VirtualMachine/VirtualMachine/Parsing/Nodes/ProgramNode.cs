
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Parsing.Nodes.Instructions;

namespace VirtualMachine.Parsing.Nodes {
    internal class ProgramNode : Node {

        public List<Node> Instructions { get; private set; }

        public Dictionary<string, int> LabelAdresses { get; private set; }

        public ProgramNode() {
            Instructions = new List<Node>();
            LabelAdresses = new Dictionary<string, int>();
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
