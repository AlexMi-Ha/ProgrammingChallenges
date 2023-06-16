
using VirtualMachine.Interpreter.Abstract;
using VirtualMachine.Lexing.Tokens;
using VirtualMachine.Parsing.Nodes;
using VirtualMachine.Parsing.Nodes.Instructions;

namespace VirtualMachine.Interpreter {
    internal class InterpreterVisitor : IInstrVisitor {

        public int Akku { get; set; }
        public Stack<int> ValueStack { get; set; } = new Stack<int>();
        public Stack<int> CallStack { get; set; } = new Stack<int>();

        public int[] Memory = new int[1024];

        public bool NegativeFlag { get; set; } = false;

        public bool SIG_HOLD = false;

        public int ProgramCounter { get; set; } = 0;

        private Dictionary<string, int> _labelAdresses;
        
        public void Visit(ProgramNode node) {
            _labelAdresses = node.LabelAdresses;
            while(!SIG_HOLD && ProgramCounter < node.Instructions.Count) {
                node.Instructions[ProgramCounter].AcceptVisitor(this);
            }
        }

        public void Visit(ArithmeticInstruction node) {
            switch(node.Type) {
                case TokenType.ADD:
                    Akku += Memory[node.Argument];
                    break;
                case TokenType.SUB:
                    Akku -= Memory[node.Argument];
                    break;
                case TokenType.MUL:
                    Akku *= Memory[node.Argument];
                    break;
                case TokenType.DIV:
                    Akku /= Memory[node.Argument];
                    break;
                case TokenType.MOD:
                    Akku %= Memory[node.Argument];
                    break;

                case TokenType.ADDC:
                    Akku += node.Argument;
                    break;
                case TokenType.SUBC:
                    Akku -= node.Argument;
                    break;
                case TokenType.MULC:
                    Akku *= node.Argument;
                    break;
                case TokenType.DIVC:
                    Akku /= node.Argument;
                    break;
                case TokenType.MODC:
                    Akku %= node.Argument;
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(IOInstruction node) {
            switch(node.Type) {
                case TokenType.IN:
                    Akku = Console.Read();
                    break;
                case TokenType.OUT:
                    Console.WriteLine(Akku);
                    break;
                case TokenType.OUTCHAR:
                    Console.WriteLine((char)Akku);
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(JumpInstruction node) {
            switch(node.Type) {
                case TokenType.JMP:
                    ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JGT:
                    if(Akku > 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JGE:
                    if (Akku >= 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JLT:
                    if (Akku < 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JLE:
                    if (Akku <= 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JEQ:
                    if (Akku == 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.JNE:
                    if (Akku != 0)
                        ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.CALL:
                    CallStack.Push(ProgramCounter + 1);
                    ProgramCounter = _labelAdresses[node.Argument!];
                    break;
                case TokenType.RETURN:
                    ProgramCounter = CallStack.Pop();
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(LabelInstruction node) { }

        public void Visit(LogicInstruction node) {
            switch(node.Type) {
                case TokenType.CMP:
                    Akku -= Memory[node.Argument];
                    break;
                case TokenType.AND:
                    Akku &= Memory[node.Argument];
                    break;
                case TokenType.OR:
                    Akku |= Memory[node.Argument];
                    break;
                case TokenType.XOR:
                    Akku ^= Memory[node.Argument];
                    break;
                case TokenType.NOT:
                    Akku = ~Akku;
                    break;
                case TokenType.SHIFTL:
                    Akku <<= Memory[node.Argument];
                    break;
                case TokenType.SHIFTR:
                    Akku >>= Memory[node.Argument];
                    break;

                case TokenType.CMPC:
                    Akku -= node.Argument;
                    break;
                case TokenType.ANDC:
                    Akku &= node.Argument;
                    break;
                case TokenType.ORC:
                    Akku |= node.Argument;
                    break;
                case TokenType.XORC:
                    Akku ^= node.Argument;
                    break;
                case TokenType.SHIFTLC:
                    Akku <<= node.Argument;
                    break;
                case TokenType.SHIFTRC:
                    Akku >>= node.Argument;
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(MemoryInstruction node) {
            switch(node.Type) {
                case TokenType.LOAD:
                    Akku = Memory[node.Argument];
                    break;
                case TokenType.STORE:
                    Memory[node.Argument] = Akku;
                    break;
                case TokenType.LOADC:
                    Akku = node.Argument;
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(StackInstruction node) {
            switch(node.Type) {
                case TokenType.PUSH:
                    ValueStack.Push(Akku);
                    break;
                case TokenType.POP:
                    Akku = ValueStack.Pop();
                    break;
                default:
                    throw new Exception($"Unexpected Operator {node.Type}");
            }
        }

        public void Visit(HoldInstruction node) {
            SIG_HOLD = true;
        }
    }
}
