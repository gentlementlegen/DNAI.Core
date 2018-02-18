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
                delegate(Entity.Variable left, Entity.Variable right)
                {
                    return left.Value || right.Value; //logical or is not overloadable
                })
        {

        }
    }
}
