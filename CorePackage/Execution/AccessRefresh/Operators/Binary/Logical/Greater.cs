using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represent ">" operator
    /// </summary>
    public class Greater : LogicalOperator
    {
        /// <summary>
        /// Constructor that need inputs types
        /// </summary>
        /// <param name="leftOpType">Type of the left operand</param>
        /// <param name="rightOpType">Type of the right operand</param>
        public Greater(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType,
                delegate(Entity.Variable left, Entity.Variable right)
                {
                    return left.Type.OperatorGt(left.Value, right.Value);
                })
        {

        }
    }
}
