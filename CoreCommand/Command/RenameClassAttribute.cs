namespace CoreCommand.Command
{
    public class RenameClassAttribute
    {
        [BinarySerializer.BinaryFormat]
        public uint ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string LastName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string NewName { get; set; }
    }
}