using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveClassAttribute : ICommand<RemoveClassAttribute.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public RemoveClassAttribute Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.RemoveClassAttribute(ClassId, Name);
            return new Reply
            {
                Command = this
            };
        }
    }
}