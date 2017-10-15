using CorePackage.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Foreach : Statement
    {
        /// <summary>
        /// Represents the array of instructions to execute
        /// </summary>
        private readonly ExecutionRefreshInstruction[] nextToExecute = new ExecutionRefreshInstruction[2];

        private enum ForeachIndexes
        {
            OUTLOOP = 0,
            INLOOP = 1
        }

        /// <summary>
        /// Current index in the collection.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Current element in the collection.
        /// </summary>
        public dynamic Element { get; private set; }

        /// <summary>
        /// Default constructor that initialises input "condition" as array and set 2 outpoints capacity
        /// </summary>
        public Foreach() :
            base(new Dictionary<string, Variable> { { "array", new Variable(new Entity.Type.ListType(Entity.Type.Scalar.Integer)) } }, 2)
        {
        }

        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            var currList = GetInputValue("array");
            if (currList?.Count > 0 && Index < currList.Count) //if foreach condition is true
            {
                //you'll have to execute recursively the nodes linked to the "in loop" index
                //then you'll have to reexecute the while
                //
                //order is reverse here because execution is performed with a stack
                nextToExecute[0] = this;
                nextToExecute[1] = this.OutPoints[(int)ForeachIndexes.INLOOP];
                Element = currList[Index];
                Index++;
            }
            else //if foreach condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = this.OutPoints[(int)ForeachIndexes.OUTLOOP];
                nextToExecute[1] = null;
            }
            return this.nextToExecute;
        }

        /// <summary>
        /// Link the instruction to the "IN LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is true</param>
        public void Do(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ForeachIndexes.INLOOP, next);
        }

        /// <summary>
        /// Link the instruction to the "OUT LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is false</param>
        public void Done(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ForeachIndexes.OUTLOOP, next);
        }
    }
}
