using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity
{
    /// <summary>
    /// Intermediary class the corresponds to a type definition
    /// </summary>
    public abstract class DataType : Global.Definition, Global.IOperable
    {
        /// <summary>
        /// Instanciate a dynamic object of the type
        /// </summary>
        /// <returns>A new instance</returns>
        public abstract dynamic Instantiate();

        /// <summary>
        /// Checks if a given value corresponds to the type
        /// </summary>
        /// <param name="value">Value to check type correspondance</param>
        /// <returns>True if value type match, false either</returns>
        public abstract bool IsValueOfType(dynamic value);

        /// <summary>
        /// Returns a deep copy of a given value
        /// </summary>
        /// <param name="value">The value to copy</param>
        /// <returns>A new instance which is equal to the given one</returns>
        public abstract dynamic GetDeepCopyOf(dynamic value, System.Type type = null);

        public abstract dynamic OperatorAdd(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorSub(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorMul(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorDiv(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorMod(dynamic lOp, dynamic rOp);

        public abstract bool OperatorGt(dynamic lOp, dynamic rOp);

        public abstract bool OperatorGtEq(dynamic lOp, dynamic rOp);

        public abstract bool OperatorLt(dynamic lOp, dynamic rOp);

        public abstract bool OperatorLtEq(dynamic lOp, dynamic rOp);

        public abstract bool OperatorEqual(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorBAnd(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorBOr(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorRightShift(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorLeftShift(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorXor(dynamic lOp, dynamic rOp);

        public abstract dynamic OperatorBNot(dynamic op);

        public abstract dynamic OperatorAccess(dynamic lOp, dynamic rOp);
    }
}
