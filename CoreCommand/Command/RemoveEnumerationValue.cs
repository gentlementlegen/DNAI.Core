namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class RemoveEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public uint EnumId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Name { get; set; }
    }
}