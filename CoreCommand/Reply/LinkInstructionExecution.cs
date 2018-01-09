namespace CoreCommand.Reply
{
    internal class LinkInstructionExecution
    {
        [BinarySerializer.BinaryFormat]
        public Command.LinkInstructionExecution Command { get; set; }
    }
}