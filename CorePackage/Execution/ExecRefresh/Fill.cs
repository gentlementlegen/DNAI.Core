using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Fill : ExecutionRefreshInstruction
    {
        public Fill(Entity.DataType stored) : base()
        {
            AddInput("array", new Entity.Variable(new Entity.Type.ListType(stored)), true);
            AddInput("element", new Entity.Variable(stored));
            AddInput("count", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddOutput("count", new Entity.Variable(Entity.Type.Scalar.Integer));
        }

        public override void Execute()
        {
            List<dynamic> value = new List<dynamic>();
            dynamic element = GetInputValue("element");
            dynamic count = GetInputValue("count");

            for (int i = 0; i < count; i++)
            {
                value.Add(element);
            }
            SetInputValue("array", value);
        }
    }
}
