namespace CoreCommand.Command
{
    public class RemoveEnumerationValue
    {
        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }
    }
}