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
        private Entity.Variable definition;

        /// <summary>
        /// Input link
        /// </summary>
        private Link link;

        /// <summary>
        /// Input name
        /// </summary>
        private string name;

        /// <summary>
        /// Is input passed by reference
        /// </summary>
        private bool reference;
        
        /// <summary>
        /// Constructor that asks for the declaration to bind
        /// </summary>
        /// <param name="value"></param>
        public Input(string name, Entity.Variable value, bool reference)
        {
            this.name = name;
            this.definition = value;
            this.reference = reference;
        }

        public Entity.Variable Definition
        {
            get
            {
                if (reference && IsLinked)
                {
                    return link.Instruction.GetOutput(link.Output).Definition;
                }
                return definition;
            }
        }

        /// <summary>
        /// Getter for the input value that refresh it with linked instruction
        /// </summary>
        public dynamic Value
        {
            get
            {
                if (IsLinked)
                {
                    if (reference)
                        return link.Value;
                    return Definition.Type.GetDeepCopyOf(link.Value);
                }

                if (reference)
                    return Definition.Value;
                return Definition.Type.GetDeepCopyOf(Definition.Value);
            }
            set
            {
                if (IsLinked)
                    throw new InvalidOperationException("You cant set value of a linked input");
                Definition.Value = value;
            }
        }
        
        /// <summary>
        /// Getter for the instruction link
        /// </summary>
        public Link Link
        {
            get { return link; }
        }

        /// <summary>
        /// Checks if an input is linked
        /// </summary>
        public bool IsLinked
        {
            get { return link != null; }
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
                throw new Error.NotFoundException("Couldn't link inexistant output named " + outputname);
            if (!linked.IsOutputCompatible(outputname, definition.Type))
                throw new InvalidOperationException("Want to link an input to an incompatible output: "+ Value.ToString() + "(" + definition.Type.ToString() + ") incompatible with " + linked.GetOutputValue(outputname) + "(" + linked.GetOutput(outputname).Definition.Type.ToString() + ")");
            link = new Link(linked, outputname);
        }

        /// <summary>
        /// Remove the link on the input
        /// </summary>
        public void Unlink()
        {
            link = null;
        }

        /// <summary>
        /// Getter for name
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
