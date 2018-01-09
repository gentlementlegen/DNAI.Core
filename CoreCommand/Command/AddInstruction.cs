using System;
using System.Collections.Generic;
using static CoreControl.InstructionFactory;

namespace CoreCommand.Command
{
    public class AddInstruction
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public INSTRUCTION_ID ToCreate { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<UInt32> Arguments { get; set; } = new List<UInt32>();
    }
}