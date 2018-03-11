using CoreControl;

namespace CoreCommand.Command
{
    public class SetListType : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint ListId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetListType(ListId, TypeId);
            return null;
        }
    }
}