using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Classify : Predict
    {
        public Classify()
        {
            AddOutput("maxOutput", new Entity.Variable(Entity.Type.Scalar.Floating));
            AddOutput("maxIndex", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        public override void Execute()
        {
            base.Execute();

            var outputs = GetOutputValue("outputs");
            var maxindex = outputs.Row(0).MaximumIndex();

            SetOutputValue("maxOutput", outputs[0, maxindex]);
            SetOutputValue("maxIndex", maxindex);
        }
    }
}
