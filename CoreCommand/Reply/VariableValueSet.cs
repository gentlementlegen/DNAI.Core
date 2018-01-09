using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Reply
{
    public class VariableValueSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetVariableValue Command { get; set; }
    }
}
