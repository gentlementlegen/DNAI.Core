namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetEnumerationType
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetEnumerationType Command { get; set; }
    }
}