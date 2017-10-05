using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represents "||" operator
    /// </summary>
    public class Or : LogicalOperator
    {
        /// <summary>
        /// Constructor that doesn't need anything cause inputs and ouputs are boolean
        /// </summary>
        public Or() :
            base(Entity.Type.Scalar.Boolean, Entity.Type.Scalar.Boolean,
                delegate(dynamic left, dynamic right)
                {
                    return left || right;
                })
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public Or(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
