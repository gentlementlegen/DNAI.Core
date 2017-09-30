using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Setter : ExecutionRefreshInstruction
    {
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

        public override void Execute()
        {
            this.outputs["reference"].Value.definition.Value = this.inputs["value"].Value.definition.Value;
        }
    }
}
