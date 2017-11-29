namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class ParentContextSet
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetContextParent Command { get; set; }
    }
}