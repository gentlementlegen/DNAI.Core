using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "-" operator : ex : int i = 3; (-i == -3)
    /// </summary>
    public class Inverse : UnaryOperator
    {
        /// <summary>
        /// Constructor that need input and output type
        /// </summary>
        /// <param name="opType">Operand type</param>
        /// <param name="resType">Returned value type</param>
        public Inverse(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(Entity.Variable op) { return -(op.Value); }, resType)
        {

        }
    }
}
