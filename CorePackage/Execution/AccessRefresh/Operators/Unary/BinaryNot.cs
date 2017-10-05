using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "~" operator
    /// </summary>
    public class BinaryNot : OverloadableUnaryOperator
    {
        /// <summary>
        /// Constructor that need input and output type
        /// </summary>
        /// <param name="opType">Type of the operand</param>
        /// <param name="resType">Type of the returned value</param>
        public BinaryNot(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(dynamic opr) { return ~opr; }, resType)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Function to overload</param>
        public BinaryNot(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
