using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Loop : Statement
    {

        /// <summary>
        /// Represents the array of instructions to execute
        /// </summary>
        protected ExecutionRefreshInstruction[] nextToExecute = new ExecutionRefreshInstruction[2];

        /// <summary>
        /// Out pin indexes for loop
        /// </summary>
        private enum LoopIndexes
        {
            INLOOP = 0,
            OUTLOOP = 1
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Loop() : base(2)
        {

        }

        /// <summary>
        /// Link the instruction to the "IN LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is true</param>
        public void Do(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)LoopIndexes.INLOOP, next);
        }

        /// <summary>
        /// Link the instruction to the "OUT LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is false</param>
        public void Done(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)LoopIndexes.OUTLOOP, next);
        }

        /// <summary>
        /// Returns the next instruction to execute in the loop
        /// </summary>
        /// <returns></returns>
        public ExecutionRefreshInstruction GetDoInstruction()
        {
            return this.ExecutionPins[(int)LoopIndexes.INLOOP];
        }

        /// <summary>
        /// Returns the next instruction to execute out of the loop
        /// </summary>
        /// <returns></returns>
        public ExecutionRefreshInstruction GetDoneInstruction()
        {
            return this.ExecutionPins[(int)LoopIndexes.OUTLOOP];
        }
    }
}
