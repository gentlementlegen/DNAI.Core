using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Equal : LogicalOperator
    {
        public Equal(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType,
                delegate(dynamic left, dynamic right)
                {
                    return left == right;
                })
        {

        }

        public Equal(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
