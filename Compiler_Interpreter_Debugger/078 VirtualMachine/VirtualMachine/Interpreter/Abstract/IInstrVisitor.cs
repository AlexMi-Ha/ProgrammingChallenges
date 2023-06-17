using VirtualMachine.Parsing.Nodes;
using VirtualMachine.Parsing.Nodes.Instructions;

namespace VirtualMachine.Interpreter.Abstract {
    internal interface IInstrVisitor {

        void Visit(ArithmeticInstruction node);
        void Visit(IOInstruction node);
        void Visit(JumpInstruction node);
        void Visit(LabelInstruction node);
        void Visit(LogicInstruction node);
        void Visit(MemoryInstruction node);
        void Visit(StackInstruction node);
        void Visit(ProgramNode node);

        void Visit(HoldInstruction node);
    }
}
