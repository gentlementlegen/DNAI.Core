using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand
{
    public class EmptyReply { }
    
    public interface ICommand<Reply>
    {
        Reply Resolve(CoreControl.Controller controller);
    }

    public class EmptyCommand : ICommand<EmptyReply>
    {
        public EmptyReply Resolve(Controller controller)
        {
            return new EmptyReply();
        }
    }
}
