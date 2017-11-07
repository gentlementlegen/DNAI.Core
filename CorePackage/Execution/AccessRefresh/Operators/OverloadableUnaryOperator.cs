using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that represents an unary operator that can be overloaded
    /// </summary>
    public class OverloadableUnaryOperator : UnaryOperator
    {
        /// <see cref="Execution.UnaryOperator"/>
        public OverloadableUnaryOperator(Entity.DataType opType, Func<dynamic, dynamic> operation, Entity.DataType resultType) :
            base(opType, operation, resultType)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public OverloadableUnaryOperator(Entity.Function overload) :
            base(
                overload.GetParameter("Operand").Type,
                delegate(dynamic op)
                {
                    overload.SetParameterValue("Operand", op);
                    overload.Call();
                    return overload.GetReturnValue("result");
                },
                overload.GetReturn("result").Type)
        {

        }
    }
}
