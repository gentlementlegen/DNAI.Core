namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public uint EnumId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public dynamic Value { get; set; }
    }
}