using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Different: LogicalOperator
    {
        public Different(Entity.DataType leftOpType, Entity.DataType rightOpType) :
            base(leftOpType, rightOpType,
                delegate(dynamic left, dynamic right)
                {
                    return left != right;
                })
        {

        }

        public Different(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
