using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "*" operator
    /// </summary>
    public class Multiplicate : OverloadableBinaryOperator
    {
        /// <summary>
        /// Constructor that 
        /// </summary>
        /// <param name="lOpType">Type of the left operand</param>
        /// <param name="rOpType">Type of the right operand</param>
        /// <param name="resType">Type of the returned value</param>
        public Multiplicate(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resType) :
            base(
                lOpType,                            //left
                rOpType,                           //right
                delegate(dynamic left, dynamic right)   //operation
                {
                    return left * right;
                },
                resType)                                 //return
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public Multiplicate(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
