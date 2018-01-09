namespace CoreCommand.Command
{
    public class SetEnumerationValue
    {
        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public dynamic Value { get; set; }
    }
}