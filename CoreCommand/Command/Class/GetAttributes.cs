using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Class
{
    public class GetAttributes : ICommand<GetAttributes.Reply>
    {
        public UInt32 ClassID { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Attributes = controller.GetClassAttributes(ClassID)
            };
        }

        public class Reply
        {
            public List<string> Attributes { get; set; }
        }

    }
}
