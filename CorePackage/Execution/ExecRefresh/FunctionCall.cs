using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Instruction that corresponds to a function call
    /// </summary>
    public class FunctionCall : ExecutionRefreshInstruction
    {
        /// <summary>
        /// Function to call
        /// </summary>
        private Entity.Function tocall;

        //add a constructor that decompose c# function to a duly function

        /// <summary>
        /// Constructor that asks for the function to call
        /// </summary>
        /// <param name="tocall">Function to call when instruction is executed</param>
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

        /// <summary>
        /// Will call the associated function
        /// </summary>
        public override void Execute()
        {
            tocall.Call();
        }
    }
}
