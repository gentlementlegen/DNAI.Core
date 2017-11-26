namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetListType
    {
        [ProtoBuf.ProtoMember(1)]
        public uint ListId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint TypeId { get; set; }
    }
}