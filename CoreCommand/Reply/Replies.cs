using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Reply
{
    public class Replies
    {
        [BinarySerializer.BinaryFormat]
        public EntityDeclared EntityDeclared { get { return new EntityDeclared(); } }
    }
}
