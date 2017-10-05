using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "+" operator
    /// </summary>
    public class Add : OverloadableBinaryOperator
    {
        /// <summary>
        /// Constructor that need inputs and output type
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="resultType">Type of the returned value</param>
        public Add(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resultType) :
            base(
                lOpType,                            //left
                rOpType,                           //right
                delegate(dynamic left, dynamic right)   //operation
                {
                    return left + right;
                },
                resultType)                                 //return
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public Add(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
