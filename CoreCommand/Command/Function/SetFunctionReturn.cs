using CoreControl;

namespace CoreCommand.Command.Function
{
    public class SetReturn : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string ExternalVarName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetFunctionReturn(FuncId, ExternalVarName);
            return null;
        }
    }
}