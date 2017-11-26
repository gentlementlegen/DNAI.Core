namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class Move
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.Move Command { get; set; }
    }
}