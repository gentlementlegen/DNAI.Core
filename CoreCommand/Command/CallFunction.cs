namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class CallFunction
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FuncId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public System.Collections.Generic.Dictionary<string, dynamic> Parameters { get; set; }
    }
}