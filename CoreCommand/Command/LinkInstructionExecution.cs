namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    internal class LinkInstructionExecution
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionID { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint FromId { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public uint OutIndex { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public uint ToId { get; set; }
    }
}