using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.Command
{
    [ProtoBuf.ProtoContract]
    public class ChangeVisibility
    {
        [ProtoBuf.ProtoMember(1)]
        EntityFactory.ENTITY EntityType { get; set; }

        [ProtoBuf.ProtoMember(2)]
        UInt32 ContainerID { get; set; }

        [ProtoBuf.ProtoMember(3)]
        string Name { get; set; }

        [ProtoBuf.ProtoMember(4)]
        EntityFactory.VISIBILITY NewVisi { get; set; }
    }
}
