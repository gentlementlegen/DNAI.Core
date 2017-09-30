using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class AccessRefreshInstruction : Instruction
    {
        public AccessRefreshInstruction(Dictionary<string, Entity.Variable> inputs, Entity.Variable output) :
            base(
                inputs,
                new Dictionary<string, Entity.Variable>
                {
                    { "result", output }
                }
            )
        {

        }

        public override Output GetOutput(string name)
        {
            Execute();
            return base.GetOutput(name);
        }
    }
}
