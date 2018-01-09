namespace CoreCommand.Command
{
    public class LinkInstructionExecution
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint FromId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint ToId { get; set; }
    }
}