namespace CoreCommand.Reply
{
    [BinarySerializer.BinaryFormat]
    internal class ParentContextSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetContextParent Command { get; set; }
    }
}