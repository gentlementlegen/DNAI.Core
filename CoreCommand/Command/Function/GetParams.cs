using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Function
{
    public class GetParams : ICommand<GetParams.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EntityId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Parameters = controller.GetFunctionParameters(EntityId)
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<EntityFactory.Entity> Parameters { get; set; }
        }
    }
}
