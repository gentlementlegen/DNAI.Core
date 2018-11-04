using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Predict : ExecutionRefreshInstruction
    {
        public Predict()
        {
            AddInput("model", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("weights", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("inputs", new Entity.Variable(Entity.Type.Matrix.Instance));
            AddOutput("outputs", new Entity.Variable(Entity.Type.Matrix.Instance));
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
