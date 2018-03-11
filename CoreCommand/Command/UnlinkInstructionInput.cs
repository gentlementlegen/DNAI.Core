using CoreControl;

namespace CoreCommand.Command
{
    public class UnlinkInstructionInput : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.UnlinkInstructionInput(FunctionID, Instruction, InputName);
            return null;
        }
    }
}