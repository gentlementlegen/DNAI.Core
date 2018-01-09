namespace CoreCommand.Command
{
    public class CallFunction
    {
        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public System.Collections.Generic.Dictionary<string, string> Parameters { get; set; }
    }
}