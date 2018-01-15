using CoreControl;

namespace CoreCommand.Command
{
    public class SetEnumerationType : ICommand<SetEnumerationType.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetEnumerationType Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetEnumerationType(EnumId, TypeId);
            return new Reply
            {
                Command = this
            };
        }
    }
}