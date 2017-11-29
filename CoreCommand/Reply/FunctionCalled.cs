namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class FunctionCalled
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.CallFunction Command { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public System.Collections.Generic.Dictionary<string, string> Value { get; set; }
    }
}