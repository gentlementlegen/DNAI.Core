namespace CoreCommand.Reply
{
    public class Move
    {
        [BinarySerializer.BinaryFormat]
        public Command.Move Command { get; set; }
    }
}