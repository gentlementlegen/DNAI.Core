using CoreControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command
{
    public class ChangeVisibility : ICommand<ChangeVisibility.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public ChangeVisibility Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.ENTITY EntityType { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY NewVisi { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.ChangeVisibility(EntityType, ContainerID, Name, NewVisi);
            return new Reply
            {
                Command = this
            };
        }
    }
}
