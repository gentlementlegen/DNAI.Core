using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetVariableType
    {
        [ProtoBuf.ProtoMember(1)]
        public UInt32 VariableID { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public UInt32 TypeID { get; set; }
    }
}
