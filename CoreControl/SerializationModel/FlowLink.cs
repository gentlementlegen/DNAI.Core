using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class FlowLink
    {
        [BinarySerializer.BinaryFormat]
        public uint OutflowInstructionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint OutflowPin { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint InflowInstructionID { get; set; }
    }
}
