using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.Command
{
    [ProtoBuf.ProtoContract]
    public class SetVariableValue
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 VariableID { get; set; }

        [ProtoBuf.ProtoMember(2, DynamicType = true)]
        public dynamic Value { get; set; }
    }
}