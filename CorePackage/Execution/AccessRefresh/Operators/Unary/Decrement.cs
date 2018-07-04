using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "--" operator
    /// </summary>
    public class Decrement : UnaryOperator
    {
        /// <summary>
        /// Constructor that need input and output type
        /// </summary>
        /// <param name="opType">Operand type</param>
        /// <param name="resType">Returned value type</param>
        public Decrement(Entity.DataType opType, Entity.DataType resType) : base(opType, (dynamic op) => --op, resType)
        {

        }
    }
}
