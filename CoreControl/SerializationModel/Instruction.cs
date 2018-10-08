using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class Instruction
    {
        [BinarySerializer.BinaryFormat]
        public InstructionFactory.INSTRUCTION_ID InstructionType { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<UInt32> Construction { get; set; }
    }
}
