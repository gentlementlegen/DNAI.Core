using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    public class AnyType : DataType
    {
        public static AnyType Instance { get; } = new AnyType();

        private AnyType()
        {

        }

        public override dynamic GetDeepCopyOf(dynamic value, System.Type type = null)
        {
            throw new NotImplementedException();
        }

        public override dynamic Instantiate()
        {
            return null;
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool IsValueOfType(dynamic value)
        {
            return true;
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBAnd(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBNot(dynamic op)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorBOr(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorDiv(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorGt(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorGtEq(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorLeftShift(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorLt(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override bool OperatorLtEq(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorMod(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }
    }
}
