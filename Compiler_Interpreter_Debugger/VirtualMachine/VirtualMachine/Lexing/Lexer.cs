
using System.Diagnostics.CodeAnalysis;
using VirtualMachine.Lexing.Abstract;
using VirtualMachine.Lexing.Machines;
using VirtualMachine.Lexing.Tokens;
using VirtualMachine.Text;

namespace VirtualMachine.Lexing {
    internal class Lexer {

        private MultilineInputReader? _input;
        private readonly List<MachineInfo> _machines;
        private Token? _currentToken;

        public Lexer() {
            _machines = new List<MachineInfo>();
            AddLexerMachines();
        }


        public Token LookAhead => _currentToken ?? Token.EOF();

        public bool Accept(TokenType type) {
            if (type == LookAhead.Type) {
                Advance();
                return true;
            }
            return false;
        }

        public Token Advance() {
            Token oldToken = LookAhead;
            do {
                _currentToken = NextWord();
            } while (_currentToken.Type is TokenType.WHITESPACE);
            return oldToken;
        }

        public Token Expect(TokenType type) {
            if (type != LookAhead.Type) {
                ThrowCompilerException($"Unexpected Token {_currentToken}", type.ToString());
            }
            return Advance();
        }


        public void Init(string input) {
            _input = new MultilineInputReader(input);
            _currentToken = null;
            Advance();
        }

        [DoesNotReturn]
        public void ThrowCompilerException(string reason, string? expected) {
            throw CreateCompilerException(reason, expected);
        }

        public Exception CreateCompilerException(string reason, string? expected) {
            if (expected is not null)
                expected = "\nExpected: " + expected;
            return new Exception($"Line {_input?.LinePos}: {reason} {expected}");
        }



        private void AddKeywordMachine(string keyword, TokenType type) {
            _machines.Add(new KeywordMachine(keyword, type));
        }

        private void InitMachines(string input) {
            foreach (var machine in _machines) {
                machine.Init(input);
            }
        }

        private Token NextWord() {
            if (_input is null) {
                throw new NullReferenceException("Input Reader is null");
            }
            if (_input.IsEmpty()) {
                return Token.EOF(_input.LinePos);
            }

            InitMachines(_input.GetRemaining());
            StepMachinesWhileActive();

            var bestMatch = GetBestMatch();

            if (bestMatch is null) {
                ThrowCompilerException($"Unexpected Token {_currentToken?.Type.ToString() ?? ""}", null);
            }


            int firstLine = _input.LinePos;
            string nextWord = _input.AdvanceAndGet(bestMatch.AcceptPos);
            return new Token {
                Line = firstLine,
                Type = bestMatch.Machine.GetTokenType(),
                Value = nextWord
            };
        }

        private void StepMachinesWhileActive() {
            int curPos = 0;
            bool active;
            do {
                active = false;
                foreach (var machine in _machines) {
                    if (machine.Machine.IsFinished()) {
                        continue;
                    }
                    active = true;
                    machine.Machine.Step();

                    if (machine.Machine.IsInFinalState()) {
                        machine.AcceptPos = curPos + 1;
                    }
                }
                ++curPos;
            } while (active);
        }

        private MachineInfo? GetBestMatch() {
            MachineInfo? bestMatch = null;
            foreach (var machine in _machines) {
                if (machine.AcceptPos > (bestMatch?.AcceptPos ?? -1)) {
                    bestMatch = machine;
                }
            }
            return bestMatch;
        }

        private void AddMachine(StateMachine machine) {
            _machines.Add(machine);
        }

        private void AddLexerMachines() {
            AddMachine(new NumberMachine());
            AddMachine(new LabelMachine());
            AddMachine(new WhitespaceMachine());
            AddKeywordMachine(":", TokenType.DOUBLECOLON);

            AddKeywordMachine("ADD", TokenType.ADD);
            AddKeywordMachine("ADDC", TokenType.ADDC);
            AddKeywordMachine("SUB", TokenType.SUB);
            AddKeywordMachine("SUBC", TokenType.SUBC);
            AddKeywordMachine("MUL", TokenType.MUL);
            AddKeywordMachine("MULC", TokenType.MULC);
            AddKeywordMachine("DIV", TokenType.DIV);
            AddKeywordMachine("DIVC", TokenType.DIVC);
            AddKeywordMachine("MOD", TokenType.MOD);
            AddKeywordMachine("MODC", TokenType.MODC);

            AddKeywordMachine("CMP", TokenType.CMP);
            AddKeywordMachine("CMPC", TokenType.CMPC);
            AddKeywordMachine("AND", TokenType.AND);
            AddKeywordMachine("ANDC", TokenType.ANDC);
            AddKeywordMachine("OR", TokenType.OR);
            AddKeywordMachine("ORC", TokenType.ORC);
            AddKeywordMachine("XOR", TokenType.XOR);
            AddKeywordMachine("XORC", TokenType.XORC);
            AddKeywordMachine("NOT", TokenType.NOT);

            AddKeywordMachine("SHIFTL", TokenType.SHIFTL);
            AddKeywordMachine("SHIFTLC", TokenType.SHIFTLC);
            AddKeywordMachine("SHIFTR", TokenType.SHIFTR);
            AddKeywordMachine("SHIFTRC", TokenType.SHIFTRC);

            AddKeywordMachine("LOAD", TokenType.LOAD);
            AddKeywordMachine("LOADC", TokenType.LOADC);
            AddKeywordMachine("STORE", TokenType.STORE);

            AddKeywordMachine("JMP", TokenType.JMP);
            AddKeywordMachine("JGT", TokenType.JGT);
            AddKeywordMachine("JGE", TokenType.JGE);
            AddKeywordMachine("JLT", TokenType.JLT);
            AddKeywordMachine("JLE", TokenType.JLE);
            AddKeywordMachine("JEQ", TokenType.JEQ);
            AddKeywordMachine("JNE", TokenType.JNE);

            AddKeywordMachine("HOLD", TokenType.HOLD);
            AddKeywordMachine("IN", TokenType.IN);
            AddKeywordMachine("OUT", TokenType.OUT);
            AddKeywordMachine("OUTCHAR", TokenType.OUTCHAR);

            AddKeywordMachine("CALL", TokenType.CALL);
            AddKeywordMachine("RETURN", TokenType.RETURN);
            AddKeywordMachine("PUSH", TokenType.PUSH);
            AddKeywordMachine("POP", TokenType.POP);
        }
    }
}
