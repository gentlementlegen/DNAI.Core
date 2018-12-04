using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Predict : ExecutionRefreshInstruction
    {
        public Predict()
        {
            AddInput("model", new Entity.Variable(Entity.Type.Resource.Instance));
            AddInput("inputs", new Entity.Variable(Entity.Type.Matrix.Instance));
            AddOutput("outputs", new Entity.Variable(Entity.Type.Matrix.Instance));
        }

        public override void Execute()
        {
            dynamic model = GetInputValue("model");
            dynamic inputs = GetInputValue("inputs");

            Global.CNTKPredict.LoadModel(model);
            SetOutputValue("outputs", Global.CNTKPredict.Predict(inputs));

            //Global.KerasService.LoadModel(model);
            //Global.KerasService.LoadWeights(weights);
            //SetOutputValue("outputs", Global.KerasService.Predict(inputs, shape));
        }
    }
}
