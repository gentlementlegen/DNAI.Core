using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    /// <summary>
    /// Instruction that represent the "&&" operator
    /// </summary>
    public class And : LogicalOperator
    {
        /// <summary>
        /// Basic constructor that doesn't need anything because inputs and output are booleans
        /// </summary>
        public And() :
            base(Entity.Type.Scalar.Boolean, Entity.Type.Scalar.Boolean,
                delegate(dynamic left, dynamic right)
                {
                    return left && right;
                })
        {

        }

        /// <summary>
        /// Constructor to overload this operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public And(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
