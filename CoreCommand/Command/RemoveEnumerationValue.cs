using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveEnumerationValue : ICommand<RemoveEnumerationValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public RemoveEnumerationValue Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.RemoveEnumerationValue(EnumId, Name);
            return new Reply
            {
                Command = this
            };
        }
    }
}