using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction that represents an unary operator wich has only one input and one output
    /// </summary>
    public class UnaryOperator : Operator
    {
        /// <summary>
        /// Constructor that need input and output type, and the operation the execute
        /// </summary>
        /// <param name="opType">Type of the operand</param>
        /// <param name="operation">Operation to execute</param>
        /// <param name="resultType">Type of the returned value</param>
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

        /// <summary>
        /// Invoke the  associated operation with the correct input value and assign the output value
        /// </summary>
        public override void Execute()
        {
            this.outputs["result"].Value.definition.Value = base.operation.DynamicInvoke(this.inputs["Operand"].Value.definition.Value);
        }
    }
}
