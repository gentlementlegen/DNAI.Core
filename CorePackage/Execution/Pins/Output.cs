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
        private Global.Declaration<Entity.Variable> value;

        /// <summary>
        /// Constructor that need the declaration to bind
        /// </summary>
        /// <param name="value">Variable declaration to bind</param>
        public Output(Global.Declaration<Entity.Variable> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Getter for the declaration
        /// </summary>
        public Global.Declaration<Entity.Variable> Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
