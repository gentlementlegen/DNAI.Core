namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class UnlinkInstructionFlow
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.UnlinkInstructionFlow Command { get; set; }
    }
}