namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class EnumerationTypeSet
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetEnumerationType Command { get; set; }
    }
}