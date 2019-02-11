using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePackage.Global;
using MathNet.Numerics.LinearAlgebra.Double;

namespace CorePackage.Execution
{
    public class LoadModel : ExecutionRefreshInstruction
    {
        public LoadModel()
        {
            AddInput("model", new Entity.Variable(Entity.Type.Resource.Instance));
        }

        public override void Execute()
        {
            dynamic model = GetInputValue("model");
            Predict.PredictorInstance.LoadModel($"{Entity.Type.Resource.Instance.Directory}/{model}");
        }
    }
}
