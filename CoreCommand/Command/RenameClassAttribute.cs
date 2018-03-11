using CoreControl;

namespace CoreCommand.Command
{
    public class RenameClassAttribute : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string LastName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string NewName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.RenameClassAttribute(ClassId, LastName, NewName);
            return null;
        }
    }
}