using System;
using System.Collections.Generic;
using static CoreControl.InstructionFactory;

namespace CoreControl.Command
{
    [ProtoBuf.ProtoContract]
    public class AddInstruction : BaseAction
    {

        [ProtoBuf.ProtoMember(5)]
        public uint Id { get; set; }

        [ProtoBuf.ProtoMember(6)]
        public List<uint> Arguments { get; set; } = new List<uint>();
    }
}