namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class GetEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint EnumId { get; set; }
    }
}