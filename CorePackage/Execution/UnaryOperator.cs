using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class UnaryOperator : Operator
    {
        public UnaryOperator(Entity.Variable operand, Delegate operation, Entity.Variable result = null):
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "Operand", operand }
                },
                (result != null ? result : new Entity.Variable(operand.Type)),
                operation
            )
        {

        }

        public override void Execute()
        {
            this.outputs["result"].Value.definition.Value = base.operation.DynamicInvoke(this.inputs["Operand"]);
        }
    }
}
