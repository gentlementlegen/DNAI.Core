using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that abstracts a statement which is not executed
    /// </summary>
    public abstract class Statement : ExecutionRefreshInstruction
    {
        /// <see cref="Execution.ExecutionRefreshInstruction"/>
        public Statement(uint outPointsCapacity): base(outPointsCapacity)
        {

        }

        /// <summary>
        /// A statement doesn't execute anything
        /// </summary>
        public override void Execute()
        {
            /* do nothing */
        }
    }
}
