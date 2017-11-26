namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class RemoveFunctionInstruction
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.RemoveFunctionInstruction Command { get; set; }
    }
}