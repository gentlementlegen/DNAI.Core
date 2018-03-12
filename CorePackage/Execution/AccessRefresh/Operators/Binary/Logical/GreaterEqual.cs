using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents ">=" operator
    /// </summary>
    public class GreaterEqual : LogicalOperator
    {
        /// <summary>
        /// Constructor that need inputs type
        /// </summary>
        /// <param name="leftOpType">Type of the left operand</param>
        /// <param name="rightOpType">Type of the right operand</param>
        public GreaterEqual(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType,
                delegate(Entity.Variable left, Entity.Variable right)
                {
                    return left.Type.OperatorGtEq(left.Value, right.Value);
                })
        {

        }
    }
}
