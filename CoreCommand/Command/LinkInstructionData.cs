namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class LinkInstructionData
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionID { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint FromId { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public string OutputName { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public uint ToId { get; set; }
        [ProtoBuf.ProtoMember(5)]
        public string InputName { get; set; }
    }
}