using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class SetValueAtKey : ExecutionRefreshInstruction
    {
        public SetValueAtKey()
        {
            var refVar = new Entity.Variable(Entity.Type.DictType.Instance);

            AddInput("reference", refVar, true);
            AddInput("key", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("value", new Entity.Variable(Entity.Type.AnyType.Instance), true);
            AddOutput("reference", refVar);
        }

        public override void Execute()
        {
            GetInput("reference").Value[GetInput("key").Value] = GetInput("value").Value;
        }
    }
}
