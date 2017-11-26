namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class LinkInstructionData
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.LinkInstructionData Command { get; set; }
    }
}