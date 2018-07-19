using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class SetValueAt : ExecutionRefreshInstruction
    {
        public SetValueAt(Entity.DataType stored) : base()
        {
            Entity.Variable val = new Entity.Variable(stored);
            AddInput("array", new Entity.Variable(new Entity.Type.ListType(stored)), true);
            AddInput("value", val);
            AddInput("index", new Entity.Variable(Entity.Type.Scalar.Integer));
            AddOutput("value", val, true);
        }

        public override void Execute()
        {
            GetInputValue("array")[GetInputValue("index")] = GetInputValue("value");
        }
    }
}
