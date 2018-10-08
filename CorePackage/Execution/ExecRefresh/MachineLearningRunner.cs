using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class MachineLearningRunner : ExecutionRefreshInstruction
    {
        public MachineLearningRunner()
        {
            AddInput("model", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("weights", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("inputs", new Entity.Variable(Entity.Type.DictType.Instance));
            AddOutput("outputs", new Entity.Variable(Entity.Type.DictType.Instance));
        }

        public override void Execute()
        {
            //convert input into json
            //launch python process
            //write input json to stdin
            //read output json from stdout
            //convert output json to dict
            throw new NotImplementedException();
        }
    }
}
