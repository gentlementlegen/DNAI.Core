using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class UnaryOperator : Operator
    {
        public UnaryOperator(Entity.DataType opType, Func<dynamic, dynamic> operation, Entity.DataType resultType):
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "Operand", new Entity.Variable(opType) }
                },
                resultType,
                operation
            )
        {

        }

        public override void Execute()
        {
            this.outputs["result"].Value.definition.Value = base.operation.DynamicInvoke(this.inputs["Operand"].Value.definition.Value);
        }
    }
}
