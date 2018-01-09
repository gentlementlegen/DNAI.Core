namespace CoreCommand.Reply
{
    public class UnlinkInstructionInput
    {
        [BinarySerializer.BinaryFormat]
        public Command.UnlinkInstructionInput Command { get; set; }
    }
}