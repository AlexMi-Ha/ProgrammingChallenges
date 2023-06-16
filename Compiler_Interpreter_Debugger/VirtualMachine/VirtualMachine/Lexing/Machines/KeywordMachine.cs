using VirtualMachine.Lexing.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Lexing.Machines {
    internal class KeywordMachine : StateMachine {

        private readonly string _keyword;
        private readonly TokenType _type;

        public KeywordMachine(string keyword, TokenType type) {
            _keyword = keyword;
            _type = type;
        }

        public override TokenType GetTokenType() {
            return _type;
        }

        protected override void InitStateTable() {
            _start = new State();
            var currentState = _start;
            for (int i = 0; i < _keyword.Length; ++i) {
                var state = new State(i == _keyword.Length - 1);
                currentState.AddTransition(state, char.ToLower(_keyword[i]), char.ToUpper(_keyword[i]));
                currentState = state;
                AddState(state);
            }
        }
    }
}
