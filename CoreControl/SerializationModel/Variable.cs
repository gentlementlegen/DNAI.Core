using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class Variable
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 Type { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }
    }
}
