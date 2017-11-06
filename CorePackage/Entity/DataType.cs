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
    public abstract class DataType : Global.Definition
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

        ///<see cref="Global.Definition.IsValid"/>
        public abstract bool IsValid();
    }
}
