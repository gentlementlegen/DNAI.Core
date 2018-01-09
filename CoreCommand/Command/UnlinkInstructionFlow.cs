namespace CoreCommand.Command
{
    public class UnlinkInstructionFlow
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutIndex { get; set; }
    }
}