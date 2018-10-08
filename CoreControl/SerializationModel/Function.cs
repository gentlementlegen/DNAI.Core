using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class Function : Context
    {
        [BinarySerializer.BinaryFormat]
        public List<string> Parameters { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<string> Returns { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<Instruction> Instructions { get; set; }
    }
}
