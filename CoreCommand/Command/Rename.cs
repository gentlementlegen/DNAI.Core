using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    public class Rename : ICommand<Rename.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public Rename Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.ENTITY EntityType { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string LastName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string NewName { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.Rename(EntityType, ContainerID, LastName, NewName);
            return new Reply
            {
                Command = this
            };
        }
    }
}
