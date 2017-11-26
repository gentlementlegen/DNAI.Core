namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class GetVariableValue
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.GetVariableValue Command { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public dynamic Value { get; set; }
    }
}