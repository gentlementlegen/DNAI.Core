namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class ChangeVisibility
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.ChangeVisibility Command { get; set; }
    }
}