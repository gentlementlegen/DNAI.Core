using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Add : OverloadableBinaryOperator
    {
        public Add(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resultType) :
            base(
                lOpType,                            //left
                rOpType,                           //right
                delegate(dynamic left, dynamic right)   //operation
                {
                    return left + right;
                },
                resultType)                                 //return
        {

        }

        public Add(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
