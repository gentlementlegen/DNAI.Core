namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class AddClassMemberFunction
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.AddClassMemberFunction Command { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint Value { get; set; }
    }
}