namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class EnumerationValueRemoved
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RemoveEnumerationValue Command { get; set; }
    }
}