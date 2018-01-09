namespace CoreCommand.Reply
{
    public class LinkInstructionData
    {
        [BinarySerializer.BinaryFormat]
        public Command.LinkInstructionData Command { get; set; }
    }
}