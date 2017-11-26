namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class RenameClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public uint ClassId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string LastName { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public string NewName { get; set; }
    }
}