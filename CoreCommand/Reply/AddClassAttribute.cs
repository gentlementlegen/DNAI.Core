namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class AddClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.AddClassAttribute Command { get; set; }
    }
}