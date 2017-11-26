namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetInstructionInputValue
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetInstructionInputValue Command { get; set; }
    }
}