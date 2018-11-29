using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class GetShape : AccessRefreshInstruction
    {
        public GetShape()
        {
            AddInput("reference", new Entity.Variable(Entity.Type.Matrix.Instance), true);
            AddOutput("ColumnCount", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddOutput("RowCount", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        public override void Execute()
        {
            SetOutputValue("ColumnCount", GetInputValue("reference").ColumnCount);
            SetOutputValue("RowCount", GetInputValue("reference").RowCount);
        }
    }
}
