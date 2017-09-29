using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class BinaryNot : OverloadableUnaryOperator
    {
        public BinaryNot(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(dynamic opr) { return ~opr; }, resType)
        {

        }

        public BinaryNot(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
