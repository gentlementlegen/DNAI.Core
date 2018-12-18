using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePackage.Global;

namespace CorePackage.Execution
{
    public class Predict : ExecutionRefreshInstruction
    {
        public static IPredictor PredictorInstance = null;
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

            PredictorInstance.LoadModel($"{Entity.Type.Resource.Instance.Directory}/{model}");
            SetOutputValue("outputs", PredictorInstance.Predict(inputs));

            //Global.KerasService.LoadModel(model);
            //Global.KerasService.LoadWeights(weights);
            //SetOutputValue("outputs", Global.KerasService.Predict(inputs, shape));
        }
    }
}
