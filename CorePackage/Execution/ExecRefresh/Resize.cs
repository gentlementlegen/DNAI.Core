using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Resize : ExecutionRefreshInstruction
    {
        public Resize()
        {
            var matrix = new Entity.Variable(Entity.Type.Matrix.Instance);

            AddInput("reference", matrix, true);
            AddInput("rowCount", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddInput("columnCount", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddInput("copy", new Entity.Variable(Entity.Type.Scalar.Boolean));
            AddOutput("result", matrix, true);
        }

        public override void Execute()
        {
            dynamic rowCount = GetInputValue("rowCount");
            dynamic colCount = GetInputValue("columnCount");
            dynamic reference = GetInputValue("reference");

            Matrix<double> result = Matrix<double>.Build.Dense(rowCount, colCount);

            if (GetInputValue("copy"))
            {
                for (int i = 0; i < rowCount && i < reference.RowCount; i++)
                {
                    for (int j = 0; j < colCount && j < reference.ColumnCount; j++)
                    {
                        result[i, j] = reference[i, j];
                    }
                }
            }

            SetOutputValue("result", result);
        }
    }
}
