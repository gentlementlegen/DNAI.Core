using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class And : LogicalOperator
    {
        public And() :
            base(Entity.Type.Scalar.Boolean, Entity.Type.Scalar.Boolean,
                delegate(dynamic left, dynamic right)
                {
                    return left && right;
                })
        {

        }

        public And(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
