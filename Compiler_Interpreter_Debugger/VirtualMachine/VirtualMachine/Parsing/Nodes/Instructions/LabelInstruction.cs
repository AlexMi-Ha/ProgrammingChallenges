
using VirtualMachine.Interpreter.Abstract;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class LabelInstruction : Node {

        public string Label { get; private set; }

        public LabelInstruction(string label) {
            Label = label;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
