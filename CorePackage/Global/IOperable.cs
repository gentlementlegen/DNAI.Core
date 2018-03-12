using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    public static class Operator
    {
        public static readonly String Left = "LeftOperand";
        public static readonly String Right = "RightOperand";
        public static readonly String Unary = "Operand";
        public static readonly String Result = "result";

        public enum Name
        {
            ADD,
            SUB,
            MUL,
            DIV,
            MOD,
            GT,
            GT_EQ,
            LT,
            LT_EQ,
            EQUAL,
            B_AND,
            B_OR,
            L_SHIFT,
            R_SHIFT,
            XOR,
            B_NOT,
            ACCESS
        };

        public enum Type
        {
            BINARY,
            UNARY
        };

        private static readonly Dictionary<Name, Type> list = new Dictionary<Name, Type>
        {
            { Name.ADD, Type.BINARY },
            { Name.SUB, Type.BINARY },
            { Name.MUL, Type.BINARY },
            { Name.DIV, Type.BINARY },
            { Name.MOD, Type.BINARY },
            { Name.GT, Type.BINARY },
            { Name.GT_EQ, Type.BINARY },
            { Name.LT, Type.BINARY },
            { Name.LT_EQ, Type.BINARY },
            { Name.EQUAL, Type.BINARY },
            { Name.B_AND, Type.BINARY },
            { Name.B_OR, Type.BINARY },
            { Name.L_SHIFT, Type.BINARY },
            { Name.R_SHIFT, Type.BINARY },
            { Name.XOR, Type.BINARY },
            { Name.B_NOT, Type.UNARY },
            { Name.ACCESS, Type.BINARY }
        };

        public static Type GetTypeOf(Name op)
        {
            if (list.ContainsKey(op))
                return list[op];
            throw new KeyNotFoundException("No such operator: " + op.ToString());
        }
    }

    public interface IOperable
    {
        //Arithmetic: + - * / %
        dynamic OperatorAdd(dynamic lOp, dynamic rOp);
        dynamic OperatorSub(dynamic lOp, dynamic rOp);
        dynamic OperatorMul(dynamic lOp, dynamic rOp);
        dynamic OperatorDiv(dynamic lOp, dynamic rOp);
        dynamic OperatorMod(dynamic lOp, dynamic rOp);

        //Comparison < > <= >= ==

        bool OperatorGt(dynamic lOp, dynamic rOp);
        bool OperatorGtEq(dynamic lOp, dynamic rOp);
        bool OperatorLt(dynamic lOp, dynamic rOp);
        bool OperatorLtEq(dynamic lOp, dynamic rOp);
        bool OperatorEqual(dynamic lOp, dynamic rOp);

        //Binaires: & | << >> ~ ^

        dynamic OperatorBAnd(dynamic lOp, dynamic rOp);
        dynamic OperatorBOr(dynamic lOp, dynamic rOp);
        dynamic OperatorRightShift(dynamic lOp, dynamic rOp);
        dynamic OperatorLeftShift(dynamic lOp, dynamic rOp);
        dynamic OperatorXor(dynamic lOp, dynamic rOp);
        dynamic OperatorBNot(dynamic op);

        //Autres: []

        dynamic OperatorAccess(dynamic lOp, dynamic rOp);
    }
}
