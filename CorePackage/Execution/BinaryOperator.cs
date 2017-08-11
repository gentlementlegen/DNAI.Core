using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class BinaryOperator : Operator
    {
        public BinaryOperator(Entity.Variable leftOperand, Entity.Variable rightOperand, Delegate operation, Entity.Variable result) :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "LeftOperand", leftOperand },
                    { "RightOperand", rightOperand }
                },
                (result != null ? result : new Entity.Variable(leftOperand.Type)),
                operation
            )
        {

        }

        public override void Execute()
        {
            this.outputs["result"].Value.definition.Value = this.operation.DynamicInvoke(this.inputs["LeftOperand"], this.inputs["RightOperand"]);
        }
    }
}
