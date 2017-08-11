using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class Operator : AccessRefreshInstruction
    {
        protected Delegate operation;
            
        public Operator(Dictionary<string, Entity.Variable> inputs, Entity.Variable output, Delegate operation):
            base(
                inputs,
                output
            )
        {
            this.operation = operation;
        }
    }
}
