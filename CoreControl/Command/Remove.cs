using System;

namespace CoreControl.Command
{
    [ProtoBuf.ProtoContract]
    public class Remove : BaseAction
    {
        [ProtoBuf.ProtoMember(1)]
        public EntityFactory.ENTITY EntityType { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public UInt32 ContainerID { get; set; }

        //[ProtoBuf.ProtoMember(3)]
        //public string Name { get; set; }
    }
}