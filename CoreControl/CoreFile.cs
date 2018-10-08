using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl
{
    public class CoreFile
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 MagicNumber { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<SerializationModel.Entity> Entities { get; set; }
    }
}
