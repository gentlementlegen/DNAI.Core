using System.Collections.Generic;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class Remove
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.Remove Command { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public List<uint> Removed { get; set; }
    }
}