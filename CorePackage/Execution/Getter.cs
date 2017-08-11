using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Getter : AccessRefreshInstruction
    {
        public Getter(Entity.Variable toget):
            base(null, toget)
        {

        }

        public override void Execute()
        {
            /* do nothing */
        }
    }
}
