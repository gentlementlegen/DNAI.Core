using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Command.Project
{
    public class Create : ICommand<Create.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public uint RootId { get; set; }
        };

        [BinarySerializer.BinaryFormat]
        public String ProjectName { get; set; }

        public Reply Resolve(CoreControl.Controller controller)
        {
            return new Reply
            {
                RootId = controller.Declare(CoreControl.EntityFactory.ENTITY.CONTEXT, 0, ProjectName, CoreControl.EntityFactory.VISIBILITY.PUBLIC)
            };
        }
    }
}
