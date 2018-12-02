using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class GetEntity : ICommand<GetEntity.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EntityId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply { Entity = controller.GetEntity(EntityId) };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public EntityFactory.Entity Entity { get; set; }
        }
    }
}
