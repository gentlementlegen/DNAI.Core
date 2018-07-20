using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represents a basic type
    /// </summary>
    public class ScalarType : DataType
    {
        /// <summary>
        /// Contains the real C# associated type
        /// </summary>
        public readonly List<System.Type> handledTypes = new List<System.Type>();

        /// <summary>
        /// Constructor that asks for the real C# type
        /// </summary>
        /// <param name="real_type">Real C# type</param>
        public ScalarType(System.Type real_type)
        {
            handledTypes.Add(real_type);
        }

        public ScalarType(System.Type toinstanciate, params System.Type[] handled)
        {
            handledTypes.Add(toinstanciate);
            foreach (System.Type curr in handled)
            {
                handledTypes.Add(curr);
            }
        }

        /// <see cref="DataType.GetDeepCopyOf(dynamic)"/>
        public override dynamic GetDeepCopyOf(dynamic value)
        {
            if (handledTypes.First() == typeof(string))
                return String.Copy(value); //need to copy for string
            return value; //scalars are automatically passed by copy
        }

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            if (handledTypes.First() == typeof(string))
                return "";
            return Activator.CreateInstance(handledTypes.First());
        }

        /// <see cref="Global.IDefinition.IsValid"/>
        public override bool IsValid()
        {
            return handledTypes.Count > 0;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            foreach (System.Type curr in handledTypes)
            {
                if (curr.IsAssignableFrom(value.GetType()))
                    return true;
            }
            return false;
        }

        /// <see cref="DataType.OperatorAccess(dynamic, dynamic)"/>
        public override dynamic OperatorAccess(dynamic lOp, dynamic rOp)
        {
            return lOp[rOp];
        }

        /// <see cref="DataType.OperatorAdd(dynamic, dynamic)"/>
        public override dynamic OperatorAdd(dynamic lOp, dynamic rOp)
        {
            return lOp + rOp;
        }

        /// <see cref="DataType.OperatorBAnd(dynamic, dynamic)"/>
        public override dynamic OperatorBAnd(dynamic lOp, dynamic rOp)
        {
            return lOp & rOp;
        }

        /// <see cref="DataType.OperatorBNot(dynamic)"/>
        public override dynamic OperatorBNot(dynamic op)
        {
            return ~op;
        }

        /// <see cref="DataType.OperatorBOr(dynamic, dynamic)"/>
        public override dynamic OperatorBOr(dynamic lOp, dynamic rOp)
        {
            return lOp | rOp;
        }

        /// <see cref="DataType.OperatorDiv(dynamic, dynamic)"/>
        public override dynamic OperatorDiv(dynamic lOp, dynamic rOp)
        {
            return lOp / rOp;
        }

        /// <see cref="DataType.OperatorEqual(dynamic, dynamic)"/>
        public override bool OperatorEqual(dynamic lOp, dynamic rOp)
        {
            return lOp.Equals(Convert.ChangeType(rOp, lOp.GetType()));
        }

        /// <see cref="DataType.OperatorGt(dynamic, dynamic)"/>
        public override bool OperatorGt(dynamic lOp, dynamic rOp)
        {
            return lOp > rOp;
        }

        /// <see cref="DataType.OperatorGtEq(dynamic, dynamic)"/>
        public override bool OperatorGtEq(dynamic lOp, dynamic rOp)
        {
            return lOp >= rOp;
        }

        /// <see cref="DataType.OperatorLeftShift(dynamic, dynamic)"/>
        public override dynamic OperatorLeftShift(dynamic lOp, dynamic rOp)
        {
            return lOp << rOp;
        }

        /// <see cref="DataType.OperatorLt(dynamic, dynamic)"/>
        public override bool OperatorLt(dynamic lOp, dynamic rOp)
        {
            return lOp < rOp;
        }

        /// <see cref="DataType.OperatorLtEq(dynamic, dynamic)"/>
        public override bool OperatorLtEq(dynamic lOp, dynamic rOp)
        {
            return lOp <= rOp;
        }

        /// <see cref="DataType.OperatorMod(dynamic, dynamic)"/>
        public override dynamic OperatorMod(dynamic lOp, dynamic rOp)
        {
            return lOp % rOp;
        }

        /// <see cref="DataType.OperatorMul(dynamic, dynamic)"/>
        public override dynamic OperatorMul(dynamic lOp, dynamic rOp)
        {
            return lOp * rOp;
        }

        /// <see cref="DataType.OperatorRightShift(dynamic, dynamic)"/>
        public override dynamic OperatorRightShift(dynamic lOp, dynamic rOp)
        {
            return lOp >> rOp;
        }

        /// <see cref="DataType.OperatorSub(dynamic, dynamic)"/>
        public override dynamic OperatorSub(dynamic lOp, dynamic rOp)
        {
            return lOp - rOp;
        }

        /// <see cref="DataType.OperatorXor(dynamic, dynamic)"/>
        public override dynamic OperatorXor(dynamic lOp, dynamic rOp)
        {
            return lOp ^ rOp;
        }
    }

    /// <summary>
    /// Static class that declares few basic types
    /// </summary>
    public static class Scalar
    {
        /// <summary>
        /// Represents a boolean type
        /// </summary>
        public static readonly ScalarType Boolean = new ScalarType(typeof(bool));

        /// <summary>
        /// Represents an integer type
        /// </summary>
        public static readonly ScalarType Integer = new ScalarType(typeof(int), typeof(long), typeof(short), typeof(ushort), typeof(uint), typeof(ulong));

        /// <summary>
        /// Represents a floating type
        /// </summary>
        public static readonly ScalarType Floating = new ScalarType(typeof(float), typeof(double));

        /// <summary>
        /// Represents a character type
        /// </summary>
        public static readonly ScalarType Character = new ScalarType(typeof(char));

        /// <summary>
        /// Represents a string type
        /// </summary>
        public static readonly ScalarType String = new ScalarType(typeof(string));
    }
}
