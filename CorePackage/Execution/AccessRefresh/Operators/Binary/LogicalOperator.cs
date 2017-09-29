using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class LogicalOperator : OverloadableBinaryOperator
    {
        public LogicalOperator(Entity.DataType lOpType, Entity.DataType rOpType, Func<dynamic, dynamic, dynamic> operation) :
            base(lOpType, rOpType, operation, Entity.Type.Scalar.Boolean)
        {

        }

        public LogicalOperator(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
