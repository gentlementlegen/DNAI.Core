using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Formula : ExecutionRefreshInstruction
    {
        public Formula(Dictionary<string, Entity.Variable> inputs, Entity.Variable output):
            base(
                inputs,
                new Dictionary<string, Entity.Variable>
                {
                    { "result", output }
                }
            )
        {

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
