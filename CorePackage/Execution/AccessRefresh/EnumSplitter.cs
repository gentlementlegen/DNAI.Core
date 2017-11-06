using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class EnumSplitter : AccessRefreshInstruction
    {
        public EnumSplitter(Entity.Type.EnumType to_split) :
            base(new Dictionary<string, Entity.Variable>(), new Dictionary<string, Entity.Variable>())
        {
            foreach (KeyValuePair<string, Entity.Variable> curr in to_split.Values)
            {
                AddOutput(new Global.Declaration<Entity.Variable> { name = curr.Key, definition = curr.Value });
            }
        }

        public override void Execute()
        {
            //do nothing
        }
    }
}
