using CoreControl;

namespace CoreCommand.Command.Class
{
    public class RemoveAttribute : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.RemoveClassAttribute(ClassId, Name);
            return null;
        }
    }
}