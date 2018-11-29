using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Random : AccessRefreshInstruction
    {
        private static readonly int NowTStamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        private static System.Random RandomManager { get; set; }

        private static int numberCreated = 0;

        public Random()
        {
            if (numberCreated % 100 == 0) // system that regenerates random
            {
                RandomManager = new System.Random(NowTStamp);
                numberCreated = 0;
            }

            AddOutput("value", new Entity.Variable(Entity.Type.Scalar.Floating, RandomManager.NextDouble()));

            ++numberCreated;
        }

        public override void Execute()
        {
            /* Do nothing */
        }
    }
}
