namespace CoreCommand.Reply
{
    public class AddInstruction
    {
        [BinarySerializer.BinaryFormat]
        public Command.AddInstruction Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Value { get; set; }
    }
}