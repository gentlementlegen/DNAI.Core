using CoreControl;

namespace CoreCommand.Command
{
    public class SetFunctionReturn : ICommand<SetFunctionReturn.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetFunctionReturn Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string ExternalVarName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetFunctionReturn(FuncId, ExternalVarName);
            return new Reply
            {
                Command = this
            };
        }
    }
}