using VirtualMachine.Lexing.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Lexing.Machines {
    internal class LabelMachine : StateMachine {
        public override TokenType GetTokenType() {
            return TokenType.LABEL;
        }

        protected override void InitStateTable() {
            _start = AddState(new State());
            var letters = AddState(new State());
            var end = AddState(new State(true));

            _start
                .AddTransitionRange(letters, 'a', 'z')
                .AddTransitionRange(letters, 'A', 'Z');

            letters
                .AddTransitionRange(letters, 'a', 'z')
                .AddTransitionRange(letters, 'A', 'Z')
                .AddTransitionRange(letters, '0', '9')
                .AddTransition(end, ':');
        }
    }
}
