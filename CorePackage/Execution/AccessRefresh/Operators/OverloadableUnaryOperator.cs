using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class OverloadableUnaryOperator : UnaryOperator
    {
        public OverloadableUnaryOperator(Entity.DataType opType, Func<dynamic, dynamic> operation, Entity.DataType resultType) :
            base(opType, operation, resultType)
        {

        }

        public OverloadableUnaryOperator(Entity.Function overload) :
            base(
                overload.Parameters[0].definition.Type,
                delegate(dynamic op)
                {
                    overload.Parameters[0].definition.Value = op;
                    overload.Call();
                    return overload.Returns[0].definition.Value;
                },
                overload.Returns[0].definition.Type)
        {

        }
    }
}
