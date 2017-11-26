using System;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class GetVariableValue
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 VariableId { get; set; }
    }
}