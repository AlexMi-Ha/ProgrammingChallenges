using VirtualMachine.Lexing;
using VirtualMachine.Parsing.Nodes;

namespace VirtualMachine.Parsing {
    internal class Parser {

        private Lexer _lexer;

        public Parser(Lexer input) {
            _lexer = input;
        }

        public ProgramNode Parse() {

        }
    }
}
