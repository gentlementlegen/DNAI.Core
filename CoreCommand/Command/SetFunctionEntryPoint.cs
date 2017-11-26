namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionEntryPoint
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint Instruction { get; set; }
    }
}