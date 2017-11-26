namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class RemoveClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public uint ClassId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Name { get; set; }
    }
}