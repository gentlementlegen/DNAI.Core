namespace CoreCommand.Reply
{
    public class VariableTypeSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetVariableType Command { get; set; }
    }
}