using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command
{
    public class LoadFrom : ICommand<LoadFrom.Reply>
    {
        public Reply Resolve(Controller controller)
        {
            return new Reply { Command = this };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public LoadFrom Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public string Filename { get; set; }
    }
}
