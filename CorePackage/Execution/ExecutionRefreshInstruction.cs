using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Class that abstract executable instruction with a parallel link system
    /// </summary>
    public abstract class ExecutionRefreshInstruction : Instruction
    {
        /// <summary>
        /// Array that contains next instructions to execute
        /// </summary>
        private ExecutionRefreshInstruction[] executionPins;

        /// <summary>
        /// Constructor that asks for inputs, outputs and out points capacity which is the maximum number of executable instruction to link
        /// </summary>
        /// <param name="inputs">Dictionarry that contains inputs</param>
        /// <param name="outputs">Dictionarry that contains outputs</param>
        /// <param name="outPointsCapacity">Maximum number of executable instruction to link</param>
        public ExecutionRefreshInstruction(uint outPointsCapacity = 1) : base()
        {
            this.executionPins = new ExecutionRefreshInstruction[outPointsCapacity];
        }

        /// <summary>
        /// Getter for out points attribute
        /// </summary>
        public ExecutionRefreshInstruction[] ExecutionPins
        {
            get { return executionPins; }
        }

        /// <summary>
        /// Used to get the next instructions to execute
        /// Can be overriden : ex : "If" need treatment here
        /// </summary>
        /// <returns>Next instructions to execute</returns>
        public virtual ExecutionRefreshInstruction[] GetNextInstructions()
        {
            return executionPins;
        }

        /// <summary>
        /// Links an executable instruction at a specific index
        /// </summary>
        /// <remarks>Throws an InvalidOperationException if index is to high</remarks>
        /// <param name="pin">Index of the pin on which link the instruction</param>
        /// <param name="instruction">Instruction to link</param>
        public void LinkTo(uint pin, ExecutionRefreshInstruction instruction)
        {
            if (pin > executionPins.Count())
                throw new InvalidOperationException("Given index is to hight");
            this.executionPins[pin] = instruction;
        }

        /// <summary>
        /// Unlink specific execution pin
        /// </summary>
        /// <remarks>Throws an InvalidOperationException if index is to high</remarks>
        /// <param name="pin">Index of the execution pin to unlink</param>
        public void Unlink(uint pin)
        {
            if (pin > executionPins.Count())
                throw new InvalidOperationException("Given index is to hight");
            this.executionPins[pin] = null;
        }
    }
}
