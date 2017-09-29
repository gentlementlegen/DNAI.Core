using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Multiplicate : OverloadableBinaryOperator
    {
        public Multiplicate(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resType) :
            base(
                lOpType,                            //left
                rOpType,                           //right
                delegate(dynamic left, dynamic right)   //operation
                {
                    return left * right;
                },
                resType)                                 //return
        {

        }

        public Multiplicate(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
