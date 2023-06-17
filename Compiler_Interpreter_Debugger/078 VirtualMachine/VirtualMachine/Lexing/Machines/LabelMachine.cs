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
                .AddTransitionRange(end, 'a', 'z')
                .AddTransitionRange(end, 'A', 'Z')
                .AddTransitionRange(end, '0', '9');

            end
                .AddTransitionRange(end, 'a', 'z')
                .AddTransitionRange(end, 'A', 'Z')
                .AddTransitionRange(end, '0', '9');
        }
    }
}
