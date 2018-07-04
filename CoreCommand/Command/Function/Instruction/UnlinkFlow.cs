using CoreControl;

namespace CoreCommand.Command.Function.Instruction
{
    public class UnlinkFlow : ICommand<EmptyReply>
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