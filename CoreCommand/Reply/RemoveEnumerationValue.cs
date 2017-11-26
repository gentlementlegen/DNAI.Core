namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class RemoveEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RemoveEnumerationValue Command { get; set; }
    }
}