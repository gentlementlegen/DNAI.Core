using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Inverse : OverloadableUnaryOperator
    {
        public Inverse(Entity.DataType opType, Entity.DataType resType) :
            base(opType, delegate(dynamic op) { return -op; }, resType)
        {

        }

        public Inverse(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
