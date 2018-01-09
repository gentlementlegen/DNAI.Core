namespace CoreCommand.Reply
{
    public class SetFunctionEntryPoint
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetFunctionEntryPoint Command { get; set; }
    }
}