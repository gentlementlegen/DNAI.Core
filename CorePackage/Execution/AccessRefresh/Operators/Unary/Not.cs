using CorePackage.Entity.Type;
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
    public class Not : UnaryOperator
    {
        /// <summary>
        /// Constructor that only need input type (assume that output is same type)
        /// </summary>
        /// <param name="opType">Operand type</param>
        public Not(Entity.DataType opType) : base(opType, (dynamic op) => !op, Scalar.Boolean)
        {

        }
    }
}
