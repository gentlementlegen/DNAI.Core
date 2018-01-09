namespace CoreCommand.Command
{
    public class UnlinkInstructionInput
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }
    }
}