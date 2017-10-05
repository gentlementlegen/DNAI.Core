using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction that represents the assignment of a variable
    /// </summary>
    public class Setter : ExecutionRefreshInstruction
    {
        /// <summary>
        /// Constructor that need the definition of the variable to set
        /// </summary>
        /// <param name="toset">Definition of the variable to set</param>
        public Setter(Entity.Variable toset):
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "value", new Entity.Variable(toset.Type) }
                },
                new Dictionary<string, Entity.Variable>
                {
                    { "reference", toset }
                }
            )
        {

        }

        /// <summary>
        /// Will set the value to the given reference
        /// </summary>
        public override void Execute()
        {
            this.outputs["reference"].Value.definition.Value = this.inputs["value"].Value.definition.Value;
        }
    }
}
