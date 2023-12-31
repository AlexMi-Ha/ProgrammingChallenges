﻿using VirtualMachine.Lexing.Abstract;
using VirtualMachine.Lexing.Tokens;

namespace VirtualMachine.Lexing.Machines {
    internal class WhitespaceMachine : StateMachine {

        public override TokenType GetTokenType() {
            return TokenType.WHITESPACE;
        }

        protected override void InitStateTable() {
            _start = AddState(new State());
            var whiteSpace = AddState(new State(true));

            _start
                .AddTransition(whiteSpace, (char)9, (char)10, (char)11, (char)12, (char)13, (char)32);

            whiteSpace
                .AddTransition(whiteSpace, (char)9, (char)10, (char)11, (char)12, (char)13, (char)32);
        }
    }
}

