using System;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionEntryPoint
    {
        [ProtoBuf.ProtoMember(1)]
        public int FunctionId { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public int Instruction { get; set; }
    }
}