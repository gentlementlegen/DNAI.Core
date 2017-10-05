using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction used to get the value of a defined variable
    /// </summary>
    public class Getter : AccessRefreshInstruction
    {
        /// <summary>
        /// Constructor that asks for the variable definition to get
        /// </summary>
        /// <param name="toget">Variable definition to get as output</param>
        public Getter(Entity.Variable toget):
            base(
                null,
                new Dictionary<string, Entity.Variable>
                {
                    { "reference", toget }
                })
        {

        }

        /// <summary>
        /// A getter doesn't need to execute anything
        /// </summary>
        public override void Execute()
        {
            /* do nothing */
        }
    }
}
