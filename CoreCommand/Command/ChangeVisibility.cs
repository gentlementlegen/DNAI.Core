using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    public class ChangeVisibility : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY NewVisi { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.ChangeVisibility(ContainerID, Name, NewVisi);
            return null;
        }
    }
}
