namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetEnumerationType
    {
        [ProtoBuf.ProtoMember(1)]
        public uint EnumId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint TypeId { get; set; }
    }
}