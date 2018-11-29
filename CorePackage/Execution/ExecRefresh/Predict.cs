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
            AddInput("weights", new Entity.Variable(Entity.Type.Resource.Instance));
            AddInput("inputs", new Entity.Variable(Entity.Type.Matrix.Instance));
            AddInput("shape", new Entity.Variable(Entity.Type.Scalar.String));
            AddOutput("outputs", new Entity.Variable(Entity.Type.Matrix.Instance));

            Global.KerasService.Init();
            SetInputValue("shape", "");
        }

        public override void Execute()
        {
            dynamic model = GetInputValue("model");
            dynamic weights = GetInputValue("weights");
            dynamic inputs = GetInputValue("inputs");
            dynamic shape = GetInputValue("shape");

            if (String.IsNullOrEmpty(shape))
            {
                shape = $"({inputs.RowCount},{inputs.ColumnCount})";
            }

            Global.KerasService.LoadModel(model);
            Global.KerasService.LoadWeights(weights);
            SetOutputValue("outputs", Global.KerasService.Predict(inputs, shape));
        }
    }
}
