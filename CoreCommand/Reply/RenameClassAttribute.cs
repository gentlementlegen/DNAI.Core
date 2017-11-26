namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class RenameClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RenameClassAttribute Command { get; set; }
    }
}