using CoreControl;

namespace CoreCommand.Command.Class
{
    public class AddAttribute : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.AddClassAttribute(ClassId, Name, TypeId, Visibility);
            return null;
        }
    }
}