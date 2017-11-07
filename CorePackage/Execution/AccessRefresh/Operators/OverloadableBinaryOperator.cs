using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that represents a binary operator that can be overloaded by a function
    /// </summary>
    public class OverloadableBinaryOperator : BinaryOperator
    {
        /// <see cref="Execution.BinaryOperator"/>
        public OverloadableBinaryOperator(Entity.DataType lOpType, Entity.DataType rOpType, Func<dynamic, dynamic, dynamic> operation, Entity.DataType resultType) :
            base(lOpType, rOpType, operation, resultType)
        {

        }

        /// <summary>
        /// Constructor used to overload the operator
        /// </summary>
        /// <param name="overload">Overload function</param>
        public OverloadableBinaryOperator(Entity.Function overload) :
            base(
                overload.GetParameter("LeftOperand").Type,      //left
                overload.GetParameter("RightOperand").Type,      //right
                delegate (dynamic left, dynamic right)   //operation
                {
                    overload.SetParameterValue("LeftOperand", left);
                    overload.SetParameterValue("RightOperand", right);
                    overload.Call();
                    return overload.GetReturnValue("result");
                },
                overload.GetReturn("result").Type)         //return
        {

        }
    }
}
