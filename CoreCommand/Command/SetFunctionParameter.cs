namespace CoreCommand.Command
{
    public class SetFunctionParameter
    {
        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string ExternalVarName { get; set; }
    }
}