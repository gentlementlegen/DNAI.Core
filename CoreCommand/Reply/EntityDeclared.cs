using System;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class EntityDeclared
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.Declare Command { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public UInt32 EntityID { get; set; }
    }
}