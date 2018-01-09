namespace CoreCommand.Command
{
    public class SetFunctionEntryPoint
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionId { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }
    }
}