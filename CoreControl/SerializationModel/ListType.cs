using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class ListType
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 StoredType { get; set; }
    }
}
