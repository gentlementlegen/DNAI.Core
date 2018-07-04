using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Clear : ExecutionRefreshInstruction
    {
        public Clear(Entity.DataType stored) : base()
        {
            AddInput("array", new Entity.Variable(new Entity.Type.ListType(stored)), true);
        }

        public override void Execute()
        {
            GetInputValue("array").Clear();
        }
    }
}
