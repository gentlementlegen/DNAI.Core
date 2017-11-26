namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class SetContextParent
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetContextParent Command { get; set; }
    }
}