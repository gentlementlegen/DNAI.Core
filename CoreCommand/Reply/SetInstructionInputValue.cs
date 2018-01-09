namespace CoreCommand.Reply
{
    public class SetInstructionInputValue
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetInstructionInputValue Command { get; set; }
    }
}