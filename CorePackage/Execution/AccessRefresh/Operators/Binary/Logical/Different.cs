using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "!=" operator
    /// </summary>
    public class Different: LogicalOperator
    {
        /// <summary>
        /// Constructor that need inputs type
        /// </summary>
        /// <param name="leftOpType">Type of the left operand</param>
        /// <param name="rightOpType">Type of the right operand</param>
        public Different(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType,
                delegate(dynamic left, dynamic right)
                {
                    return left != right;
                })
        {

        }

        /// <summary>
        /// Constructor to overload the operator with a function
        /// </summary>
        /// <param name="overload">Overload function</param>
        public Different(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
