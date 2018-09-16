using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction that currently prints its inputs on debug output
    /// </summary>
    public class Debug : ExecutionRefreshInstruction
    {
        /// <summary>
        /// Need an array of inputs to print
        /// </summary>
        /// <param name="inputs">Dictionarry that contains inputs</param>
        public Debug(Dictionary<string, Entity.Variable> inputs): base()
        {
            AddInputs(inputs);
        }

        /// <summary>
        /// Constructor to print only one variable
        /// </summary>
        /// <param name="toprint">Variable to watch</param>
        public Debug(Entity.Variable toprint) : base()
        {
            AddInput("to_print", toprint, true);
            GetInput("to_print").IsValueSet = true;
        }

        /// <summary>
        /// Will show a message on the standard output
        /// </summary>
        public override void Execute()
        {
            string message = "Debug: " + GetInputValue("to_print").ToString();
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
