using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class Debug : ExecutionRefreshInstruction
    {
        public Debug(Dictionary<string, Entity.Variable> inputs):
            base(inputs, null)
        {

        }
    }
}
