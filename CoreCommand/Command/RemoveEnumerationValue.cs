using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveEnumerationValue : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.RemoveEnumerationValue(EnumId, Name);
            return null;
        }
    }
}