using CoreControl;

namespace CoreCommand.Command
{
    public class RemoveClassAttribute : ICommand<EmptyReply>
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