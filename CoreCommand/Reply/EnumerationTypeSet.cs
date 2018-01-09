namespace CoreCommand.Reply
{
    public class EnumerationTypeSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetEnumerationType Command { get; set; }
    }
}