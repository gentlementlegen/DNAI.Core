using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that represents a binary operator that have 2 inputs and 1 output
    /// </summary>
    public class BinaryOperator : Operator
    {
        /// <summary>
        /// Constructor that need inputs and outputs type, and the operation to execute
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="operation">Operation to execute</param>
        /// <param name="resultType">Type of the returned value</param>
        public BinaryOperator(Entity.DataType lOpType, Entity.DataType rOpType, Func<Entity.Variable, Entity.Variable, dynamic> operation, Entity.DataType resultType) :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { Global.Operator.Left, new Entity.Variable(lOpType) },
                    { Global.Operator.Right, new Entity.Variable(rOpType) }
                },
                resultType,
                operation
            )
        {

        }

        /// <summary>
        /// Invoke the operation with correct input values
        /// </summary>
        public override void Execute()
        {
            this.outputs[Global.Operator.Result].Value.definition.Value = this.operation.DynamicInvoke(this.inputs[Global.Operator.Left].Value.definition, this.inputs[Global.Operator.Right].Value.definition);
        }
    }
}
