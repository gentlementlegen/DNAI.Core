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

        public Output(Global.Declaration<Entity.Variable> value)
        {
            this.value = value;
        }

        public Global.Declaration<Entity.Variable> Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
