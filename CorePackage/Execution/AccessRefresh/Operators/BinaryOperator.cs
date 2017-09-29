using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class BinaryOperator : Operator
    {
        public BinaryOperator(Entity.DataType lOpType, Entity.DataType rOpType, Func<dynamic, dynamic, dynamic> operation, Entity.DataType resultType) :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "LeftOperand", new Entity.Variable(lOpType) },
                    { "RightOperand", new Entity.Variable(rOpType) }
                },
                resultType,
                operation
            )
        {

        }

        public override void Execute()
        {
            this.outputs["result"].Value.definition.Value = this.operation.DynamicInvoke(this.inputs["LeftOperand"].Value.definition.Value, this.inputs["RightOperand"].Value.definition.Value);
        }
    }
}
