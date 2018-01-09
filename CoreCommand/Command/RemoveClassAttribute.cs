namespace CoreCommand.Command
{
    public class RemoveClassAttribute
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }
    }
}