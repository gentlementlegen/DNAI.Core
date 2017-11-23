using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class Rename
    {
        [ProtoBuf.ProtoMember(1)]
        public EntityFactory.ENTITY EntityType { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public UInt32 ContainerID { get; set; }

        [ProtoBuf.ProtoMember(3)]
        public string LastName { get; set; }

        [ProtoBuf.ProtoMember(4)]
        public string NewName { get; set; }
    }
}
