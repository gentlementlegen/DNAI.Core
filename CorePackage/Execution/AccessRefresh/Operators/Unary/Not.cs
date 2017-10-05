using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "!" operator
    /// </summary>
    public class Not : OverloadableUnaryOperator
    {
        /// <summary>
        /// Constructor that only need input type (assume that output is same type)
        /// </summary>
        /// <param name="opType">Operand type</param>
        public Not(Entity.DataType opType) :
            base(
                opType,
                delegate(dynamic op)
                {
                    return !op;
                },
                Entity.Type.Scalar.Boolean)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Function to overload</param>
        public Not(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
