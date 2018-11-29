using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class Load : ICommand<Load.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<UInt32> Projects { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public string Filename { get; set; }
        
        public Reply Resolve(Controller controller)
        {
            controller.LoadFrom(Filename);

            var projs = new List<UInt32>();
            List<EntityFactory.Entity> entities = controller.GetEntitiesOfType(EntityFactory.ENTITY.CONTEXT, 0);

            foreach (EntityFactory.Entity entity in entities)
            {
                projs.Add(entity.Id);
            }

            return new Reply
            {
                Projects = projs
            };
        }
    }
}
