
namespace VirtualMachine.Lexing.Tokens {
    internal class Token {

        public required TokenType Type { get; init; }

        public string? Value { get; init; }

        public required int Line { get; init; }


        public override string ToString() {
            return $"{Type} {Value}";
        }

        public static Token EOF() {
            return EOF(0);
        }

        public static Token EOF(int linePos) {
            return new Token {
                Type = TokenType.EOF,
                Value = string.Empty,
                Line = linePos,
            };
        }
    }
}
