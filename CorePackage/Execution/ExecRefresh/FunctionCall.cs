using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class FunctionCall : ExecutionRefreshInstruction
    {
        private Entity.Function tocall;

        public FunctionCall(Entity.Function tocall) :
            base(
                null,
                null
            )
        {
            foreach (Global.Declaration<Entity.Variable> curr in tocall.Parameters)
            {
                AddInput(curr);
            }

            foreach (Global.Declaration<Entity.Variable> curr in tocall.Returns)
            {
                AddOutput(curr);
            }

            this.tocall = tocall;
        }

        public override void Execute()
        {
            tocall.Call();
        }
    }
}
