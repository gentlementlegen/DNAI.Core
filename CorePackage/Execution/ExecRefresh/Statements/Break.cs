using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Break : Statement
    {
        public Break() : base(0)
        {

        }

        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            return new ExecutionRefreshInstruction[0];
        }
    }
}
