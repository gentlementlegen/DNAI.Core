using CoreControl;

namespace CoreCommand.Command
{
    public class UnlinkInstructionInput : ICommand<UnlinkInstructionInput.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UnlinkInstructionInput Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.UnlinkInstructionInput(FunctionID, Instruction, InputName);
            return new Reply
            {
                Command = this
            };
        }
    }
}