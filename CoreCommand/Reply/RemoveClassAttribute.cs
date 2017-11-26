namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class RemoveClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RemoveClassAttribute Command { get; set; }
    }
}