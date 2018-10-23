using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class DataLink
    {
        [BinarySerializer.BinaryFormat]
        public uint OutputInstructionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Output { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint InputInstructionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Input { get; set; }
    }
}
