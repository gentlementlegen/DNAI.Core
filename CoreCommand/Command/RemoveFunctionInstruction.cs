using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveFunctionInstruction : ICommand<RemoveFunctionInstruction.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public RemoveFunctionInstruction Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionId { get; internal set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; internal set; }

        public Reply Resolve(Controller controller)
        {
            controller.RemoveFunctionInstruction(FunctionId, Instruction);
            return new Reply
            {
                Command = this
            };
        }
    }
}