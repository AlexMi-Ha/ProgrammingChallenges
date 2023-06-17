
namespace VirtualMachine.Lexing.Abstract {
    internal class MachineInfo {
        public required StateMachine Machine { get; init; }
        public int AcceptPos { get; set; } = 0;

        public void Init(string input) {
            AcceptPos = 0;
            Machine.Init(input);
        }

        public static implicit operator MachineInfo(StateMachine machine) => new MachineInfo { Machine = machine };
    }
}
