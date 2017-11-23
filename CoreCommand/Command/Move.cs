using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class Move
    {
        [ProtoBuf.ProtoMember(1)]
        EntityFactory.ENTITY EntityType { get; set; }

        [ProtoBuf.ProtoMember(2)]
        UInt32 FromID { get; set; }

        [ProtoBuf.ProtoMember(3)]
        UInt32 ToID { get; set; }

        [ProtoBuf.ProtoMember(4)]
        string Name { get; set; }
    }
}
