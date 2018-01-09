using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand
{
    public class ProtoData
    {
        [BinarySerializer.BinaryFormat]
        Command.Commands Commands { get { return new Command.Commands(); } }

        [BinarySerializer.BinaryFormat]
        Reply.Replies Replies { get { return new Reply.Replies(); } }
    }
}
