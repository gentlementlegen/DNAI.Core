using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command.Declarator
{
    public class Move : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 FromID { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ToID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.Move(FromID, ToID, Name);
            return null;
        }
    }
}
