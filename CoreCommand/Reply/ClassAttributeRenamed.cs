namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class ClassAttributeRenamed
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RenameClassAttribute Command { get; set; }
    }
}