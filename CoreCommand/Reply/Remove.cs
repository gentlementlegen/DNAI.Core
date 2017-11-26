namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class Remove
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.Remove Command { get; set; }
    }
}