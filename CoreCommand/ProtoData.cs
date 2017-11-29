using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand
{
    [ProtoBuf.ProtoContract]
    public class ProtoData
    {
        [ProtoBuf.ProtoMember(1)]
        Command.Commands Commands { get { return new Command.Commands(); } }

        [ProtoBuf.ProtoMember(2)]
        Reply.Replies Replies { get { return new Reply.Replies(); } }
    }
}
