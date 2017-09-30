using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    /// <summary>
    /// Represents a declaration which means the association of a name and a definition
    /// The declaration is here to retreive a definition from its name
    /// </summary>
    public class Declaration<DefinitionType>
    {
        /// <summary>
        /// Represents the name of the declared node
        /// </summary>
        public string name;

        /// <summary>
        /// Represents the definition of the declared node
        /// </summary>
        public DefinitionType definition;
    }
}
