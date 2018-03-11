using CoreControl;

namespace CoreCommand.Command
{
    public class UnlinkInstructionFlow : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.UnlinkInstructionFlow(FunctionID, Instruction, OutIndex);
            return null;
        }
    }
}