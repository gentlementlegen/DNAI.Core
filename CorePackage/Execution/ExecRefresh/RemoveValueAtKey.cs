using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class RemoveValueAtKey : ExecutionRefreshInstruction
    {
        public RemoveValueAtKey()
        {
            var refVal = new Entity.Variable(Entity.Type.DictType.Instance);

            AddInput("reference", refVal, true);
            AddInput("key", new Entity.Variable(Entity.Type.Scalar.String));
            AddOutput("reference", refVal, true);
        }

        public override void Execute()
        {
            GetInput("reference").Value.Remove(GetInput("key").Value);
        }
    }
}
