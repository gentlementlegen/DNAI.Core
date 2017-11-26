namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class AddInstruction
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.AddInstruction Command { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public uint Value { get; set; }
    }
}