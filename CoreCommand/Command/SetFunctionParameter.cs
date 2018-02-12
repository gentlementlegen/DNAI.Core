using CoreControl;

namespace CoreCommand.Command
{
    public class SetFunctionParameter : ICommand<SetFunctionParameter.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetFunctionParameter Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string ExternalVarName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetFunctionParameter(FuncId, ExternalVarName);
            return new Reply
            {
                Command = this
            };
        }
    }
}