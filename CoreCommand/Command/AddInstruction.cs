using System;
using System.Collections.Generic;
using static CoreControl.InstructionFactory;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class AddInstruction
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 FunctionID { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public INSTRUCTION_ID ToCreate { get; set; }

        [ProtoBuf.ProtoMember(3)]
        public List<UInt32> Arguments { get; set; } = new List<UInt32>();
    }
}