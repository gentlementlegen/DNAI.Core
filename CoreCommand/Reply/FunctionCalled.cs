namespace CoreCommand.Reply
{
    public class FunctionCalled
    {
        [BinarySerializer.BinaryFormat]
        public Command.CallFunction Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public System.Collections.Generic.Dictionary<string, string> Value { get; set; }
    }
}