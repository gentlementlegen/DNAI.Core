using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class RemoveProject : ICommand<RemoveProject.Reply>
    {
        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Removed = controller.Remove(0, ProjectName)
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<UInt32> Removed { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public String ProjectName { get; set; }
    }
}
