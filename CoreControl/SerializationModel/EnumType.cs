using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class EnumType
    {
        [BinarySerializer.BinaryFormat]
        public Dictionary<string, string> Values { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 StoredType { get; set; }
    }
}
