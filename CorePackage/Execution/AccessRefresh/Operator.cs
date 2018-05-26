using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that represents an operator which need inputs, outputs and operation
    /// </summary>
    public abstract class Operator : AccessRefreshInstruction
    {
        /// <summary>
        /// The operation to execute
        /// </summary>
        protected Delegate operation;
       
        /// <summary>
        /// Constructor that need inputs, outputs and operation
        /// </summary>
        /// <param name="inputs">Dictionnary of all inputs</param>
        /// <param name="outputType">Dictionnary of all outputs</param>
        /// <param name="operation">Operation to execute</param>
        public Operator(Entity.DataType outputType, Delegate operation): base()
        {
            AddOutput(Global.Operator.Result, new Entity.Variable(outputType));
            this.operation = operation;
        }
    }
}
