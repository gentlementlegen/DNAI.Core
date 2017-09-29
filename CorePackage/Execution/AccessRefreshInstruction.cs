using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class AccessRefreshInstruction : Instruction
    {
        public AccessRefreshInstruction(Dictionary<string, Entity.Variable> inputs, Dictionary<string, Entity.Variable> outputs) :
            base(
                inputs,
                outputs
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
