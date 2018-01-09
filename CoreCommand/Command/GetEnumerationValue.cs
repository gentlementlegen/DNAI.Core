namespace CoreCommand.Command
{
    public class GetEnumerationValue
    {
        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }
    }
}