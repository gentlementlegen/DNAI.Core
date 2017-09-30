using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class If : Statement
    {
        public enum ConditionIndexes : int
        {
            OnTrue = 0,
            OnFalse = 1
        }

        ExecutionRefreshInstruction[] nextinstrution = new ExecutionRefreshInstruction[1];

        public If():
            base(
                new Dictionary<string, Entity.Variable>
                {
                    { "condition", new Entity.Variable(Entity.Type.Scalar.Boolean, false) }
                },
                2
            )
        {

        }

        public override ExecutionRefreshInstruction[] GetNextInstructions()
        {
            nextinstrution[0] = this.OutPoints[this.GetInput("condition").Value.definition.Value == true ? (int)ConditionIndexes.OnTrue : (int)ConditionIndexes.OnFalse];
            return nextinstrution;
        }

        public void Then(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ConditionIndexes.OnTrue, next);
        }

        public void Else(ExecutionRefreshInstruction next)
        {
            this.LinkTo((int)ConditionIndexes.OnFalse, next);
        }
    }
}
