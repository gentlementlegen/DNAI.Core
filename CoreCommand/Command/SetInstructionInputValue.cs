namespace CoreCommand.Command
{
    public class SetInstructionInputValue
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputValue { get; set; }
    }
}