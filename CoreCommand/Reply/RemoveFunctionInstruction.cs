namespace CoreCommand.Reply
{
    public class RemoveFunctionInstruction
    {
        [BinarySerializer.BinaryFormat]
        public Command.RemoveFunctionInstruction Command { get; set; }
    }
}