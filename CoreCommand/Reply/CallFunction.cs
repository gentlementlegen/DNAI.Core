namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class CallFunction
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.CallFunction Command { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public System.Collections.Generic.Dictionary<string, dynamic> Value { get; set; }
    }
}