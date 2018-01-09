using CoreControl;

namespace CoreCommand.Command
{
    public class AddClassAttribute
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }
    }
}