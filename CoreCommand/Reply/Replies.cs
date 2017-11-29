using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class Replies
    {
        [ProtoBuf.ProtoMember(1)]
        public EntityDeclared EntityDeclared { get { return new EntityDeclared(); } }
    }
}
