using CoreControl;
using System.Collections.Generic;

namespace CoreCommand.Command.Function
{
    public class GetReturns : ICommand<GetReturns.Reply>
    {
        public uint EntityId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Returns = controller.GetFunctionReturns(EntityId)
            };
        }

        public class Reply
        {
            public List<EntityFactory.Entity> Returns { get; set; }
        }
    }
}
