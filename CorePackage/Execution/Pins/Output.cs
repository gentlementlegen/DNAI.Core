using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Represents an output
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Reference a variable declaration as value
        /// </summary>
        private Entity.Variable definition;

        /// <summary>
        /// Constructor that need the declaration to bind
        /// </summary>
        /// <param name="value">Variable declaration to bind</param>
        public Output(Entity.Variable definition)
        {
            this.definition = definition;
        }

        public Entity.Variable Definition
        {
            get
            {
                return definition;
            }
        }

        /// <summary>
        /// Getter for the declaration
        /// </summary>
        public dynamic Value
        {
            get
            {
                return definition.Value;
            }
            set
            {
                definition.Value = value;
            }
        }
    }
}
