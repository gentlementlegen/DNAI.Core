using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    public class SetVariableType
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 TypeID { get; set; }
    }
}
