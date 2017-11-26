using CoreControl;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class AddClassAttribute
    {
        [ProtoBuf.ProtoMember(1)]
        public uint ClassId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public string Name { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public uint TypeId { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public EntityFactory.VISIBILITY Visibility { get; set; }
    }
}