namespace CoreCommand.Command
{
    public class SetEnumerationType
    {
        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }
    }
}