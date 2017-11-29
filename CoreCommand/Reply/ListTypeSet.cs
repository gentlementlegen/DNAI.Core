namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class ListTypeSet
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetListType Command { get; set; }
    }
}