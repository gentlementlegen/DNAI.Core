using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    /// <summary>
    /// Represents a definition object
    /// </summary>
    public interface IDefinition
    {
        /// <summary>
        /// Checks if a definition content is valid
        /// </summary>
        /// <returns>True if it's valid, false either</returns>
        bool IsValid();

        string Name { get; set; }

        String FullName { get; }

        IDeclarator Parent { get; set; }
    }
}
