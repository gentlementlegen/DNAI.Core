using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StorageDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Variable>;

namespace CorePackage.Execution
{
    /// <summary>
    /// Represent an instruction node
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// Intruction as internal context where inputs and outputs
        /// references <c>context.externals</c> declarations
        /// </summary>
        private StorageDeclarator scope = new StorageDeclarator();

        /// <summary>
        /// Instruction inputs that reference <c>context.externals</c> declaration
        /// </summary>
        protected Dictionary<string, Input> inputs = new Dictionary<string, Input>();

        /// <summary>
        /// Instruction outputs that reference <c>context.externals</c> declaration
        /// </summary>
        protected Dictionary<string, Output> outputs = new Dictionary<string, Output>();

        /// <summary>
        /// Constructor that asks for inputs and outputs
        /// </summary>
        /// <param name="inputs">Dictionarry that contains all inputs</param>
        /// <param name="outputs">Dictionarry that contains all outputs</param>
        public Instruction(Dictionary<string, Entity.Variable> inputs = null, Dictionary<string, Entity.Variable> outputs = null)
        {            
            if (inputs != null)
                foreach (KeyValuePair<string, Entity.Variable> curr in inputs)
                {
                    AddInput(curr.Key, curr.Value);
                }

            if (outputs != null)
                foreach (KeyValuePair<string, Entity.Variable> curr in outputs)
                {
                    AddOutput(curr.Key, curr.Value);
                }
        }

        /// <summary>
        /// Getter for inputs attribute
        /// </summary>
        public List<Input> Inputs
        {
            get { return inputs.Values.ToList(); }
        }

        /// <summary>
        /// Getter for outputs attribute
        /// </summary>
        public List<Output> Outputs
        {
            get { return outputs.Values.ToList(); }
        }

        /// <summary>
        /// Allow to add input to the instruction
        /// </summary>
        /// <param name="name">Name of the input</param>
        /// <param name="definition">Variable definition of the input</param>
        public void AddInput(string name, Entity.Variable definition)
        {
            AddInput(new Global.Declaration<Entity.Variable>{name = name, definition = scope.Declare(definition, name, Global.AccessMode.EXTERNAL)});
        }

        /// <summary>
        /// Allow to add an input from a declaration
        /// </summary>
        /// <param name="declaration">Declaration linked to the input</param>
        public void AddInput(Global.Declaration<Entity.Variable> declaration)
        {
            this.inputs[declaration.name] = new Input(declaration);
        }

        /// <summary>
        /// Allow to add an output to the instruction
        /// </summary>
        /// <param name="name">Name of the output</param>
        /// <param name="definition">Variable definition of the output</param>
        public void AddOutput(string name, Entity.Variable definition)
        {
            AddOutput(new Global.Declaration<Entity.Variable> { name = name, definition = scope.Declare(definition, name, Global.AccessMode.EXTERNAL) });
        }

        /// <summary>
        /// Allow to add an output from a declaration
        /// </summary>
        /// <param name="declaration">Declaration to bind to the input</param>
        public void AddOutput(Global.Declaration<Entity.Variable> declaration)
        {
            this.outputs[declaration.name] = new Output(declaration);
        }

        /// <summary>
        /// Allow to get an input from its name
        /// </summary>
        /// <remarks>Throws an Error.NotFoundException is not found</remarks>
        /// <param name="name">Name of the input to get</param>
        /// <returns>The input associated with the name</returns>
        public Input GetInput(string name)
        {
            if (!this.inputs.ContainsKey(name))
                throw new Error.NotFoundException("No such input named: " + name);
            return this.inputs[name];
        }

        /// <summary>
        /// Allow to get an output from its name
        /// </summary>
        /// <param name="name">Name of the output to get</param>
        /// <remarks>Throws an Error.NotFoundException is not found</remarks>
        /// <returns>The output associated with the name</returns>
        public virtual Output GetOutput(string name)
        {
            if (!this.outputs.ContainsKey(name))
                throw new Error.NotFoundException("No such output named: " + name);
            return this.outputs[name];
        }

        /// <summary>
        /// Allow to set a value to a specific input
        /// </summary>
        /// <param name="name">Name of the input to set</param>
        /// <param name="value">Value to set to the input</param>
        public void SetInputValue(string name, dynamic value)
        {
            GetInput(name).Value.definition.Value = value;
        }

        /// <summary>
        /// Allow to get the value of a specific input
        /// </summary>
        /// <param name="name">Name of the input from which retreive the value</param>
        /// <returns>Value of the input</returns>
        public dynamic GetInputValue(string name)
        {
            return this.GetInput(name).Value.definition.Value;
        }

        /// <summary>
        /// Allow to get the value of a specific output
        /// </summary>
        /// <param name="name">Name of the output from which find the value</param>
        /// <returns>Desired output value</returns>
        public dynamic GetOutputValue(string name)
        {
            return this.GetOutput(name).Value.definition.Value;
        }

        /// <summary>
        /// Checks if an output exists in instruction
        /// </summary>
        /// <param name="name">Output name to check existance</param>
        /// <returns>True if output exists, false either</returns>
        public bool HasOutput(string name)
        {
            return (outputs.ContainsKey(name));
        }

        /// <summary>
        /// Checks if an input exists in instruction
        /// </summary>
        /// <param name="name">Input name to check existance</param>
        /// <returns>True if input exists, false either</returns>
        public bool HasInput(string name)
        {
            return inputs.ContainsKey(name);
        }

        /// <summary>
        /// Abstract method used to execute the instruction
        /// </summary>
        public abstract void Execute();
    }
}
