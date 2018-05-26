using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Formula : ExecutionRefreshInstruction
    {
  
        public Formula(Dictionary<string, Entity.Variable> inputs, Entity.Variable output): base()
        {
            foreach (KeyValuePair<string, Entity.Variable> input in inputs)
            {
                AddInput(input.Key, input.Value);
            }
            AddOutput("result", output);
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
