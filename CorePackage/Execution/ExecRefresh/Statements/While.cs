using System.Collections.Generic;

namespace CorePackage.Execution
{
    /// <summary>
    /// While loop statement. Executes instruction until condition is completed.
    /// </summary>
    public class While : Loop
    {        
        /// <summary>
        /// Default constructor that initialises input "condition" as boolean and set 2 outpoints capacity
        /// </summary>
        public While() : base()
        {
            AddInput("condition", new Entity.Variable(Entity.Type.Scalar.Boolean));
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
                nextToExecute[1] = GetDoInstruction();
            }
            else //if while condition is false
            {
                //you only have to execute the code "out loop"
                nextToExecute[0] = GetDoneInstruction();
                nextToExecute[1] = null;
            }
            return this.nextToExecute;
        }
    }
}