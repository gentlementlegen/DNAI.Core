using System;
using System.Collections.Generic;

namespace CoreControl
{
    public class CoreFile
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 MagicNumber { get; set; }

        [BinarySerializer.BinaryFormat]
        public SerializationModel.Version Version { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<SerializationModel.Entity> Entities { get; set; }
    }
}
