using CoreControl;

namespace CoreCommand.Command.Function
{
    public class RemoveInstruction : ICommand<EmptyReply>
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