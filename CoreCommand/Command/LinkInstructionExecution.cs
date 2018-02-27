using CoreControl;

namespace CoreCommand.Command
{
    public class LinkInstructionExecution : ICommand<LinkInstructionExecution.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public LinkInstructionExecution Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.LinkInstructionExecution(FunctionID, FromId, OutIndex, ToId);
            return new Reply
            {
                Command = this
            };
        }
    }
}