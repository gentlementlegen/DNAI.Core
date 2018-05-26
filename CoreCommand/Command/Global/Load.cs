using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class Load : ICommand<Load.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<UInt32> Projects { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public string Filename { get; set; }
        
        public Reply Resolve(Controller controller)
        {
            return null;
        }
    }
}
