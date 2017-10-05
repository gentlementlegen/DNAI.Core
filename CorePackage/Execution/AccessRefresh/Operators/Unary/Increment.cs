using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "++" operator
    /// </summary>
    public class Increment: OverloadableUnaryOperator
    {
        /// <summary>
        /// Constructor that need input and output type
        /// </summary>
        /// <param name="opType">Operand type</param>
        /// <param name="resType">Returned value type</param>
        public Increment(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(dynamic op) { return ++op; }, resType)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Function to overload</param>
        public Increment(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
