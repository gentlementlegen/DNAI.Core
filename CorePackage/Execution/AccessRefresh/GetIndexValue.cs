using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class GetIndexValue : AccessRefreshInstruction
    {
        public GetIndexValue()
        {
            AddInput("reference", new Entity.Variable(Entity.Type.Matrix.Instance), true);
            AddInput("row", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddInput("column", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddOutput("value", new Entity.Variable(Entity.Type.Scalar.Floating));
        }

        public override void Execute()
        {
            dynamic matrix = GetInputValue("reference");
            dynamic i = GetInputValue("row"), j = GetInputValue("column");

            SetOutputValue("value", matrix[i, j]);
        }
    }
}
