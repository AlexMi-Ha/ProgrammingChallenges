
namespace VirtualMachine.Lexing {
    internal static class LexerFactory {

        public static Lexer CreateLexer(string input) {
            var lex = new Lexer();
            lex.Init(input);
            return lex;
        }
    }
}
