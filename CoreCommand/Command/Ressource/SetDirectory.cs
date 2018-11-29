using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Ressource
{
    public class SetDirectory : ICommand<EmptyReply>
    {
        public string Directory { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetRessourceDirectory(Directory);
            return null;
        }
    }
}
