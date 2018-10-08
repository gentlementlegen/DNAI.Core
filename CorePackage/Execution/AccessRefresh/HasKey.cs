using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class HasKey : AccessRefreshInstruction
    {
        public HasKey()
        {
            AddInput("reference", new Entity.Variable(Entity.Type.DictType.Instance), true);
            AddInput("key", new Entity.Variable(Entity.Type.Scalar.String));
            AddOutput("result", new Entity.Variable(Entity.Type.Scalar.Boolean));
        }

        public override void Execute()
        {
            SetOutputValue("result", GetInput("reference").Value.ContainsKey(GetInput("key").Value));
        }
    }
}
