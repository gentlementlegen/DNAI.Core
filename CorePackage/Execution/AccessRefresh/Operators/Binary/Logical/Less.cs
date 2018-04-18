using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "<" operator
    /// </summary>
    public class Less : LogicalOperator
    {
        /// <summary>
        /// Constructor that need inputs type
        /// </summary>
        /// <param name="leftOpType">Type of the left operand</param>
        /// <param name="rightOpType">Type of the right operand</param>
        public Less(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType, (dynamic left, dynamic right) => leftOpType.OperatorLt(left, right))
        {
             
        }
    }
}
