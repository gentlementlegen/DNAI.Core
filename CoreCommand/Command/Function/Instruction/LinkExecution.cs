using CoreControl;

namespace CoreCommand.Command.Function.Instruction
{
    public class LinkExecution : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.LinkInstructionExecution(FunctionID, FromId, OutIndex, ToId);
            return null;
        }
    }
}