namespace CoreCommand.Command
{
    public class RemoveFunctionInstruction
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionId { get; internal set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; internal set; }
    }
}