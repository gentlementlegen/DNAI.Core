using CoreControl;

namespace CoreCommand.Command.Function
{
    public class SetParameter : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string ExternalVarName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetFunctionParameter(FuncId, ExternalVarName);
            return null;
        }
    }
}