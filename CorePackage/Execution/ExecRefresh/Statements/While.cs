using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// While loop statement. Executes instruction until condition is completed.
    /// </summary>
    public class While : Statement
    {
        /// <summary>
        /// Represents the array of instructions to execute
        /// </summary>
        private ExecutionRefreshInstruction[] nextToExecute = new ExecutionRefreshInstruction[2];

        /// <summary>
        /// Enumeration that tells which index for which system
        /// </summary>
        private enum WhileIndexes
        {
            OUTLOOP = 0,
            INLOOP = 1
        }

        /// <summary>
        /// Default constructor that initialises input "condition" as boolean and set 2 outpoints capacity
        /// </summary>
        public While() :
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "condition", new Entity.Variable(Entity.Type.Scalar.Boolean) }
                },
                null,
                2
            )
        {
        }

        /// <see cref="ExecutionRefreshInstruction.GetNextInstructions"/>
        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            if (this.GetInputValue("condition")) //if while condition is true
            {
                //you'll have to execute recursively the nodes linked to the "in loop" index
                //then you'll have to reexecute the while
                //
                //order is reverse here because execution is performed with a stack
                nextToExecute[0] = this;
                nextToExecute[1] = this.OutPoints[(int)WhileIndexes.INLOOP];
            }
            else //if while condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = this.OutPoints[(int)WhileIndexes.OUTLOOP];
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
            this.LinkTo((int)WhileIndexes.INLOOP, next);
        }

        /// <summary>
        /// Link the instruction to the "OUT LOOP" index
        /// </summary>
        /// <param name="next">Instruction to execute if while condition is false</param>
        public void Done(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)WhileIndexes.OUTLOOP, next);
        }
    }
}