using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "^"
    /// </summary>
    public class Xor : OverloadableBinaryOperator
    {
        /// <summary>
        /// Constructor that 
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="resType">Type of the returned value</param>
        public Xor(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resType) :
            base(lOpType, rOpType,
                delegate(dynamic left, dynamic right)
                {
                    return left ^ right;
                }, resType)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public Xor(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
