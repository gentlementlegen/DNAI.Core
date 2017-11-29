namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class ClassAttributeAdded
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.AddClassAttribute Command { get; set; }
    }
}