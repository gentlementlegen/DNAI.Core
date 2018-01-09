namespace CoreCommand.Reply
{
    internal class ClassAttributeAdded
    {
        [BinarySerializer.BinaryFormat]
        public Command.AddClassAttribute Command { get; set; }
    }
}