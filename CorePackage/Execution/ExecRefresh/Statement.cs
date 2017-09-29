using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class Statement : ExecutionRefreshInstruction
    {
        public Statement(Dictionary<string, Entity.Variable> inputs, uint outPointsCapacity):
            base(inputs, null, outPointsCapacity)
        {

        }

        public override void Execute()
        {
            /* do nothing */
        }
    }
}
