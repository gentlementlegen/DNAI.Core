namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class UnlinkInstructionFlow
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionID { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint Instruction { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public uint OutIndex { get; set; }
    }
}