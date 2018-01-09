namespace CoreCommand.Reply
{
    public class ClassMemberFunctionAdded
    {
        [BinarySerializer.BinaryFormat]
        public Command.AddClassMemberFunction Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Value { get; set; }
    }
}