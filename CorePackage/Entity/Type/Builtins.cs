using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Entity.Type
{
    public static class Builtins
    {
        /// <summary>
        /// Represents a boolean type
        /// </summary>
        public static readonly ScalarType Boolean = Scalar.Boolean;

        /// <summary>
        /// Represents an integer type
        /// </summary>
        public static readonly ScalarType Integer = Scalar.Integer;

        /// <summary>
        /// Represents a floating type
        /// </summary>
        public static readonly ScalarType Floating = Scalar.Floating;

        /// <summary>
        /// Represents a character type
        /// </summary>
        public static readonly ScalarType Character = Scalar.Character;

        /// <summary>
        /// Represents a string type
        /// </summary>
        public static readonly ScalarType String = Scalar.String;

        /// <summary>
        /// Represents the dictionnary type
        /// </summary>
        public static readonly DictType Dictionnary = DictType.Instance;

        /// <summary>
        /// Represents the any generic type
        /// </summary>
        public static readonly AnyType Any = AnyType.Instance;

        /// <summary>
        /// Represents the matrix type
        /// </summary>
        public static readonly Matrix Matrix = Matrix.Instance;

        /// <summary>
        /// Represents the resource type
        /// </summary>
        public static readonly Resource Resource = Resource.Instance;
    }
}
