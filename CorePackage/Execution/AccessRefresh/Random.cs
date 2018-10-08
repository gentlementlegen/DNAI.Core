using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Random : AccessRefreshInstruction
    {
        private System.Random RandomManager { get; set; } = new System.Random();

        public Random()
        {
            AddOutput("value", new Entity.Variable(Entity.Type.Scalar.Floating));
        }

        public override void Execute()
        {
            SetOutputValue("value", RandomManager.NextDouble());
        }
    }
}
