namespace CoreCommand.Reply
{
    public class SetFunctionReturn
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetFunctionReturn Command { get; set; }
    }
}