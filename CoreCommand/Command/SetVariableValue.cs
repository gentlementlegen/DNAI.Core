using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    public class SetVariableValue
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }
    }
}