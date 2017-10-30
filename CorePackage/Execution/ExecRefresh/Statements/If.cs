using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction that can split execution in 2 branches
    /// </summary>
    public class If : Statement
    {
        /// <summary>
        /// Enum to represents which index of outPoints list is for true condition and false execution
        /// </summary>
        public enum ConditionIndexes : int
        {
            OnTrue = 0,
            OnFalse = 1
        }

        /// <summary>
        /// Array that will contain the nextInstruction to execute
        /// </summary>
        ExecutionRefreshInstruction[] nextinstrution = new ExecutionRefreshInstruction[1];

        /// <summary>
        /// Default constructor
        /// </summary>
        public If():
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "condition", new Entity.Variable(Entity.Type.Scalar.Boolean, false) }
                },
                null,
                2
            )
        {

        }

        /// <summary>
        /// Choose right instruction to execute and return it
        /// </summary>
        /// <returns>The array nextInstructions with the first index set with the instruction to execute</returns>
        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            nextinstrution[0] = this.OutPoints[this.GetInput("condition").Value.definition.Value == true ? (int)ConditionIndexes.OnTrue : (int)ConditionIndexes.OnFalse];
            return nextinstrution;
        }

        /// <summary>
        /// Links an instruction to execute when condition is true
        /// </summary>
        /// <param name="next">Instruction to link</param>
        public void Then(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ConditionIndexes.OnTrue, next);
        }

        /// <summary>
        /// Links an instruction to execute when condition is false
        /// </summary>
        /// <param name="next">Instruction to link</param>
        public void Else(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ConditionIndexes.OnFalse, next);
        }
    }
}
