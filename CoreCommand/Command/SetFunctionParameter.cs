namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionParameter
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FuncId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string ExternalVarName { get; set; }
    }
}