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
                overload.Parameters[0].definition.Type,      //left
                overload.Parameters[1].definition.Type,      //right
                delegate (dynamic left, dynamic right)   //operation
                {
                    overload.Parameters[0].definition.Value = left;
                    overload.Parameters[0].definition.Value = right;
                    overload.Call();
                    return overload.Returns[0].definition.Value;
                },
                overload.Returns[0].definition.Type)         //return
        {

        }
    }
}
