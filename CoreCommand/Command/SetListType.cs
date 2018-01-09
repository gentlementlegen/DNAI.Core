namespace CoreCommand.Command
{
    public class SetListType
    {
        [BinarySerializer.BinaryFormat]
        public uint ListId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint TypeId { get; set; }
    }
}