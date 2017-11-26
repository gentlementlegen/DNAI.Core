namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class RemoveFunctionInstruction
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionId { get; internal set; }
        [ProtoBuf.ProtoMember(2)]
        public uint Instruction { get; internal set; }
    }
}