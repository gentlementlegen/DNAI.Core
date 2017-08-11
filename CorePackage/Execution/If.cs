using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class If : Statement
    {
        public If():
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "condition", new Entity.Variable(Entity.Type.Scalar.Boolean, false) }
                },
                2
            )
        {

        }

        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            throw new NotImplementedException();
        }
    }
}
