using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class ObjectType : Context
    {
        [BinarySerializer.BinaryFormat]
        public Dictionary<string, UInt32> Attributes { get; set; }
    }
}
