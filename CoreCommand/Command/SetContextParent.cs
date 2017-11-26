using System;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetContextParent
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 ContextId { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public UInt32 ParentId { get; set; }
    }
}