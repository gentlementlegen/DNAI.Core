using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that corresponds to the "|" operator
    /// </summary>
    public class BinaryOr : BinaryOperator
    {
        /// <summary>
        /// Constructor that 
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="resType">Type of the returned value</param>
        public BinaryOr(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resType) :
            base(lOpType, rOpType, (dynamic left, dynamic right) => lOpType.OperatorBOr(left, right), resType)
        {

        }
    }
}
