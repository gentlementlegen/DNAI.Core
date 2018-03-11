using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveFunctionInstruction : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionId { get; internal set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; internal set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.RemoveFunctionInstruction(FunctionId, Instruction);
            return null;
        }
    }
}