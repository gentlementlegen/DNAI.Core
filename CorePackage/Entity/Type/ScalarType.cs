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

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            if (handledTypes.First() == typeof(string))
                return "";
            return Activator.CreateInstance(handledTypes.First());
        }

        /// <see cref="Global.Definition.IsValid"/>
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
        public static readonly ScalarType Integer = new ScalarType(typeof(long), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(ulong));

        /// <summary>
        /// Represents a floating type
        /// </summary>
        public static readonly ScalarType Floating = new ScalarType(typeof(double), typeof(float));

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
