namespace CoreCommand.Reply
{
    public class ClassAttributeRenamed
    {
        [BinarySerializer.BinaryFormat]
        public Command.RenameClassAttribute Command { get; set; }
    }
}