using CoreControl;

namespace CoreCommand.Command
{
    public class AddClassAttribute : ICommand<AddClassAttribute.Reply>
    {

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public AddClassAttribute Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.AddClassAttribute(ClassId, Name, TypeId, Visibility);
            return new Reply
            {
                Command = this
            };
        }
    }
}