using CoreControl;

namespace CoreCommand.Command
{
    public class LinkInstructionData : ICommand<LinkInstructionData.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public LinkInstructionData Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string OutputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.LinkInstructionData(FunctionID, FromId, OutputName, ToId, InputName);
            return new Reply
            {
                Command = this
            };
        }
    }
}