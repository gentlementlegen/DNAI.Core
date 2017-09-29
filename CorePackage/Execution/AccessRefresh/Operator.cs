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
            
        public Operator(Dictionary<string, Entity.Variable> inputs, Entity.DataType outputType, Delegate operation):
            base(
                inputs,
                new Dictionary<string, Entity.Variable>
                {
                    { "result", new Entity.Variable(outputType) }
                }
            )
        {
            this.operation = operation;
        }
    }
}
