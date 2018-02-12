using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command
{
    public class SerializeTo : ICommand<SerializeTo.Reply>
    {
        public Reply Resolve(Controller controller)
        {
            return new Reply { Command = this };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SerializeTo Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public string Filename { get; set; }
    }
}
