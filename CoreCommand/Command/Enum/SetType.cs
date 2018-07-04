using CoreControl;

namespace CoreCommand.Command.Enum
{
    public class SetType : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetEnumerationType(EnumId, TypeId);
            return null;
        }
    }
}