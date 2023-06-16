
using VirtualMachine.Interpreter.Abstract;

namespace VirtualMachine.Parsing.Nodes.Instructions {
    internal class LiteralNode : Node {

        public int Value {  get; private set; }

        public LiteralNode(int val) {
            Value = val;
        }

        public override void AcceptVisitor(IInstrVisitor visitor) {
            visitor.Visit(this);
        }
    }
}
