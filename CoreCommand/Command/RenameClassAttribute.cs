using CoreControl;

namespace CoreCommand.Command
{
    public class RenameClassAttribute : ICommand<RenameClassAttribute.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public RenameClassAttribute Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string LastName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string NewName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.RenameClassAttribute(ClassId, LastName, NewName);
            return new Reply
            {
                Command = this
            };
        }
    }
}