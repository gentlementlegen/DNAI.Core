namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetListType
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetListType Command { get; set; }
    }
}