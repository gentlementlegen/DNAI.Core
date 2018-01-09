namespace CoreCommand.Reply
{
    public class RemoveClassAttribute
    {
        [BinarySerializer.BinaryFormat]
        public Command.RemoveClassAttribute Command { get; set; }
    }
}