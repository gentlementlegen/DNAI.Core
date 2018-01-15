using CoreControl;

namespace CoreCommand.Command
{
    public class UnlinkInstructionFlow : ICommand<UnlinkInstructionFlow.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UnlinkInstructionFlow Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.UnlinkInstructionFlow(FunctionID, Instruction, OutIndex);
            return new Reply
            {
                Command = this
            };
        }
    }
}