using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetVariableValue
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 VariableID { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string Value { get; set; }
    }
}