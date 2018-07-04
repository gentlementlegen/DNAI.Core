using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class GetProjectEntities : ICommand<GetProjectEntities.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public String ProjectName { get; set; }

        public Reply Resolve(Controller controller)
        {
            //need getEntityID
            //need getEntities
            return new Reply
            {
                //Entities = controller.GetEntitiesOfType(EntityFactory.ENTITY.CONTEXT, )
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<EntityFactory.Entity> Entities { get; set; }
        }
    }
}
