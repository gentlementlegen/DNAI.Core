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
                this.inputs[curr.name] = new Input(curr);
            }

            foreach (Global.Declaration<Entity.Variable> curr in tocall.Returns)
            {
                this.outputs[curr.name] = new Output(curr);
            }

            this.tocall = tocall;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
