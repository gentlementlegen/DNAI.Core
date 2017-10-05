using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that abstracts an instruction which is refresh when something wants be access output value
    /// </summary>
    public abstract class AccessRefreshInstruction : Instruction
    {
        /// <see cref="Instruction"/>
        public AccessRefreshInstruction(Dictionary<string, Entity.Variable> inputs, Dictionary<string, Entity.Variable> outputs) :
            base(
                inputs,
                outputs
            )
        {

        }

        /// <summary>
        /// Get output value from its name, will also execute the node
        /// </summary>
        /// <param name="name">Name of the output to get</param>
        /// <returns>The output needed</returns>
        public override Output GetOutput(string name)
        {
            Execute();
            return base.GetOutput(name);
        }
    }
}
