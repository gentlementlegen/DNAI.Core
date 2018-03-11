using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand
{
    public class EmptyReply { }

    public interface ICommand<Reply>
    {
        Reply Resolve(CoreControl.Controller controller);
    }
}
