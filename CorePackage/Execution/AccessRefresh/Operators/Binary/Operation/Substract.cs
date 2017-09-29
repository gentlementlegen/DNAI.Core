using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Substract : OverloadableBinaryOperator
    {
        public Substract(Entity.DataType lOpType, Entity.DataType rOpType, Entity.DataType resType) :
            base(lOpType, rOpType, delegate(dynamic left, dynamic right) { return left - right; }, resType)
        {

        }

        public Substract(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
