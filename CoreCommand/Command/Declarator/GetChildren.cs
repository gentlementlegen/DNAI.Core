using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Declarator
{
    public class GetChildren : ICommand<GetChildren.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EntityId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Children = controller.GetChildren(EntityId)
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<uint> Children { get; set; }
        }
    }
}
