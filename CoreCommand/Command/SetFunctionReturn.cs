using CoreControl;

namespace CoreCommand.Command
{
    public class SetFunctionReturn : ICommand<EmptyReply>
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