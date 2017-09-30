using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    /// <summary>
    /// Represent an input
    /// </summary>
    public class Input
    {
        /// <summary>
        /// Reference a variable declaration
        /// </summary>
        private Global.Declaration<Entity.Variable> value;

        /// <summary>
        /// Reference an output to set the <c>value</c>
        /// Can be null to keep default <c>value</c>
        /// </summary>
        private Instruction linkedInstruction;

        private string linkedOutputName;

        public Input(Global.Declaration<Entity.Variable> value)
        {
            this.value = value;
        }

        public Global.Declaration<Entity.Variable> Value
        {
            get
            {
                if (this.linkedInstruction != null && this.linkedOutputName != null)
                    value.definition.Value = this.linkedInstruction.GetOutput(this.linkedOutputName).Value.definition.Value;
                return value;
            }
        }

        public void LinkTo(Instruction linked, string outputname)
        {
            this.linkedInstruction = linked;
            this.linkedOutputName = outputname;
        }

        public void Unlink()
        {
            this.linkedInstruction = null;
            this.linkedOutputName = null;
        }
    }
}
