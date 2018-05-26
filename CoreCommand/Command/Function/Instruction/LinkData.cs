using CoreControl;

namespace CoreCommand.Command.Function.Instruction
{
    public class LinkData : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string OutputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.LinkInstructionData(FunctionID, FromId, OutputName, ToId, InputName);
            return null;
        }
    }
}