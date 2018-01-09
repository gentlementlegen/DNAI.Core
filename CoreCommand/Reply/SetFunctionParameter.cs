namespace CoreCommand.Reply
{
    public class SetFunctionParameter
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetFunctionParameter Command { get; set; }
    }
}