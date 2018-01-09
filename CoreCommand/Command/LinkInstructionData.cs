namespace CoreCommand.Command
{
    public class LinkInstructionData
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string OutputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }
    }
}