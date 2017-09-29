using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Debug : ExecutionRefreshInstruction
    {
        public Debug(Dictionary<string, Entity.Variable> inputs):
            base(inputs, null)
        {

        }

        public Debug(Entity.Variable toprint) :
            base(null, null)
        {
            AddInput("to_print", toprint);
        }

        public override void Execute()
        {
            string message = "Debug: " + GetInput("to_print").Value.definition.Value.ToString();
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
