using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction used to split all enum values as output
    /// </summary>
    public class EnumSplitter : AccessRefreshInstruction
    {
        /// <summary>
        /// Constructor that asks for an enum to split
        /// </summary>
        /// <param name="to_split">Enum to split</param>
        public EnumSplitter(Entity.Type.EnumType to_split) :
            base(new Dictionary<string, Entity.Variable>(), to_split.Values)
        {

        }

        /// <summary>
        /// Execute function that does nothing
        /// </summary>
        public override void Execute()
        {
            //do nothing
        }
    }
}
