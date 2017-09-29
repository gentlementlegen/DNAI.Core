using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Increment: OverloadableUnaryOperator
    {
        public Increment(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(dynamic op) { return ++op; }, resType)
        {

        }

        public Increment(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
