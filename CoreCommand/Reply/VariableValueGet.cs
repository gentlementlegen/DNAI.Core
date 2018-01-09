namespace CoreCommand.Reply
{
    public class VariableValueGet
    {
        [BinarySerializer.BinaryFormat]
        public Command.GetVariableValue Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }
    }
}