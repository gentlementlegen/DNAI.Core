namespace CoreCommand.Reply
{
    public class ListTypeSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetListType Command { get; set; }
    }
}