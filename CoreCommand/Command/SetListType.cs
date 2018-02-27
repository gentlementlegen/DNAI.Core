using CoreControl;

namespace CoreCommand.Command
{
    public class SetListType : ICommand<SetListType.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetListType Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint ListId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetListType(ListId, TypeId);
            return new Reply
            {
                Command = this
            };
        }
    }
}