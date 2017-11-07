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

        /// <summary>
        /// Name of the output linked
        /// </summary>
        private string linkedOutputName;

        /// <summary>
        /// Getter for linked instruction
        /// </summary>
        public Instruction LinkedInstruction
        {
            get { return linkedInstruction; }
        }

        /// <summary>
        /// Getter for linked output name
        /// </summary>
        public string LinkedOutputName
        {
            get { return linkedOutputName; }
        }

        /// <summary>
        /// Constructor that asks for the declaration to bind
        /// </summary>
        /// <param name="value"></param>
        public Input(Global.Declaration<Entity.Variable> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Getter for the input value that refresh it with linked instruction
        /// </summary>
        public Global.Declaration<Entity.Variable> Value
        {
            get
            {
                if (this.linkedInstruction != null && this.linkedOutputName != null)
                    value.definition.Value = this.linkedInstruction.GetOutput(this.linkedOutputName).Value.definition.Value;
                return value;
            }
        }

        /// <summary>
        /// Link the input to an instruction at a specific output
        /// </summary>
        /// <remarks>Throws an Error.NotFoundException is not found</remarks>
        /// <param name="linked">Instruction to link</param>
        /// <param name="outputname">Output name of the instruction to link</param>
        public void LinkTo(Instruction linked, string outputname)
        {
            if (!linked.HasOutput(outputname))
                throw new Error.NotFoundException("Input.LinkTo : Input " + value.name + " : No such output named " + outputname);
            this.linkedInstruction = linked;
            this.linkedOutputName = outputname;
        }

        /// <summary>
        /// Remove the link on the input
        /// </summary>
        public void Unlink()
        {
            this.linkedInstruction = null;
            this.linkedOutputName = null;
        }
    }
}
