using VirtualMachine.Interpreter.Abstract;

namespace VirtualMachine.Parsing.Nodes {
    internal abstract class Node {

        public abstract void AcceptVisitor(IInstrVisitor visitor);

        public int Line { get; set; }
    }
}
