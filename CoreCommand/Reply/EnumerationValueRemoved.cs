namespace CoreCommand.Reply
{
    public class EnumerationValueRemoved
    {
        [BinarySerializer.BinaryFormat]
        public Command.RemoveEnumerationValue Command { get; set; }
    }
}