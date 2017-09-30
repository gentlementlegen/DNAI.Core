using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public abstract class ExecutionRefreshInstruction : Instruction
    {
        private ExecutionRefreshInstruction[] outPoints;

        public ExecutionRefreshInstruction(Dictionary<string, Entity.Variable> inputs = null, Dictionary<string, Entity.Variable> outputs = null, uint outPointsCapacity = 1) :
            base(inputs, outputs)
        {
            this.outPoints = new ExecutionRefreshInstruction[outPointsCapacity];
        }

        public ExecutionRefreshInstruction[] OutPoints
        {
            get { return outPoints; }
        }

        public virtual ExecutionRefreshInstruction[] GetNextInstructions()
        {
            return outPoints;
        }

        public void LinkTo(uint index, ExecutionRefreshInstruction outPoint)
        {
            this.outPoints[index] = outPoint;
        }
    }
}
