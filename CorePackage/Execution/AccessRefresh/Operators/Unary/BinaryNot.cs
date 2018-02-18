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
    public class BinaryNot : UnaryOperator
    {
        /// <summary>
        /// Constructor that need input and output type
        /// </summary>
        /// <param name="opType">Type of the operand</param>
        /// <param name="resType">Type of the returned value</param>
        public BinaryNot(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(Entity.Variable opr) { return opr.Type.OperatorBNot(opr.Value); }, resType)
        {

        }
    }
}
