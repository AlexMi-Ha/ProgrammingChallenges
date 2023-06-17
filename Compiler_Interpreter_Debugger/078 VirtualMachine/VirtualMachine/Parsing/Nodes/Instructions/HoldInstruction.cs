using VirtualMachine.Interpreter.Abstract;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class HoldInstruction : Node {
        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
