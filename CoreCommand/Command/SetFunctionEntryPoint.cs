using CoreControl;

namespace CoreCommand.Command
{
    public class SetFunctionEntryPoint : ICommand<SetFunctionEntryPoint.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetFunctionEntryPoint Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetFunctionEntryPoint(FunctionId, Instruction);
            return new Reply
            {
                Command = this
            };
        }
    }
}