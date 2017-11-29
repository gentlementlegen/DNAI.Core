namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class VariableValueGet
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.GetVariableValue Command { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string Value { get; set; }
    }
}