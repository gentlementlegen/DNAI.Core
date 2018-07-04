using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command.Declarator
{
    public class Rename : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string LastName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string NewName { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.Rename(ContainerID, LastName, NewName);
            return null;
        }
    }
}
