using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution.Operators
{
    public class Not : OverloadableUnaryOperator
    {
        public Not(Entity.DataType opType) :
            base(
                opType,
                delegate(dynamic op)
                {
                    return !op;
                },
                Entity.Type.Scalar.Boolean)
        {

        }

        public Not(Entity.Function overload) :
            base(overload)
        {

        }
    }
}
