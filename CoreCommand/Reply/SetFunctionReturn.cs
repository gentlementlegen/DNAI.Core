namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionReturn
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetFunctionReturn Command { get; set; }
    }
}