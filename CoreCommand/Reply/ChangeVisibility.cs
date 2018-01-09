namespace CoreCommand.Reply
{
    public class ChangeVisibility
    {
        [BinarySerializer.BinaryFormat]
        public Command.ChangeVisibility Command { get; set; }
    }
}