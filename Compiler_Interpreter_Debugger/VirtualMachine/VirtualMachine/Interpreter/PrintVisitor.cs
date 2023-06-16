
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;
using VirtualMachine.Parsing.Nodes;
using VirtualMachine.Parsing.Nodes.Instructions;

namespace VirtualMachine.Interpreter {
    internal class PrintVisitor : IInstrVisitor {

        public void Visit(ArithmeticInstruction node) {
            Console.WriteLine($"\t{node.Type} {node.Argument}");
        }

        public void Visit(IOInstruction node) {
            Console.WriteLine("\t"+node.Type);
        }

        public void Visit(JumpInstruction node) {
            Console.WriteLine($"\t{node.Type} {node.Argument ?? ""}");
        }

        public void Visit(LabelInstruction node) {
            Console.WriteLine(node.Label);
        }

        public void Visit(LogicInstruction node) {
            string arg = node.Type is TokenType.NOT ? "" : node.Argument.ToString();
            Console.WriteLine($"\t{node.Type} {arg}");
        }

        public void Visit(MemoryInstruction node) {
            Console.WriteLine($"\t{node.Type} {node.Argument}");
        }

        public void Visit(StackInstruction node) {
            Console.WriteLine($"\t{node.Type}");
        }

        public void Visit(ProgramNode node) {
            Console.WriteLine("entry:");
        }

        public void Visit(HoldInstruction node) {
            Console.WriteLine("HOLD");
        }
    }
}
