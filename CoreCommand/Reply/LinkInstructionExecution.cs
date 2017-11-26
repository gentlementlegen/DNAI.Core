namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class LinkInstructionExecution
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.LinkInstructionExecution Command { get; set; }
    }
}