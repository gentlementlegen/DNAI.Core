namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class UnlinkInstructionInput
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.UnlinkInstructionInput Command { get; set; }
    }
}