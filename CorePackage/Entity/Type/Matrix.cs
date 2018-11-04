using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    public class Matrix : DataType
    {
        public static Matrix Instance { get; } = new Matrix();

        private Matrix()
        {
            /*
             * Get Shape
             * Resize
             * Get index
             */
        }

        public override dynamic GetDeepCopyOf(dynamic value, System.Type type = null)
        {
            Matrix<double> cpy = Matrix<double>.Build.DenseOfColumns(value);

            value.CopyTo(cpy);
            return cpy;
        }

        public override dynamic Instantiate()
        {
            return Matrix<double>.Build.Dense(10, 10);
        }

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public override bool IsValueOfType(dynamic value)
        {
            return value.GetType() == typeof(Matrix<double>);
        }

        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            return lOp.Column(rOp);
        }

        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            return lOp.Add(rOp);
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
            return lOp.Divide(rOp);
        }

        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            return lOp.Equals(rOp);
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
            return lOp.Modulus(rOp);
        }

        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            return lOp.Multiply(rOp);
        }

        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }

        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            return lOp.Subtract(rOp);
        }

        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            throw new NotImplementedException();
        }
    }
}
