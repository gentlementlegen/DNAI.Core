using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Class that represents all of the logic operators with has only one boolean output
    /// </summary>
    public class LogicalOperator : BinaryOperator
    {
        /// <summary>
        /// Constructor that need inputs types and the operation to process
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="operation">Operation to execute</param>
        public LogicalOperator(Entity.DataType lOpType, Entity.DataType rOpType, Func<dynamic, dynamic, dynamic> operation) :
            base(lOpType, rOpType, operation, Entity.Type.Scalar.Boolean)
        {

        }
    }
}
