using CorePackage.Entity;
using CorePackage.Global;
using System.Collections.Generic;
using System.Linq;

namespace CorePackage.Execution
{
    /// <summary>
    /// Represent an instruction node
    /// </summary>
    public abstract class Instruction
    {
        /// <summary>
        /// Instruction inputs that reference <c>context.externals</c> declaration
        /// </summary>
        private Dictionary<string, Input> inputs = new Dictionary<string, Input>();

        /// <summary>
        /// Instruction outputs that reference <c>context.externals</c> declaration
        /// </summary>
        private Dictionary<string, Output> outputs = new Dictionary<string, Output>();

        /// <summary>
        /// Getter for inputs attribute
        /// </summary>
        public Dictionary<string, Input> Inputs
        {
            get { return inputs; }
        }

        /// <summary>
        /// Getter for outputs attribute
        /// </summary>
        public Dictionary<string, Output> Outputs
        {
            get { return outputs; }
        }

        /// <summary>
        /// Allow to add input to the instruction
        /// </summary>
        /// <param name="name">Name of the input</param>
        /// <param name="definition">Variable definition of the input</param>
        public void AddInput(string name, Entity.Variable definition)
        {
            this.inputs[name] = new Input(definition);
        }

        /// <summary>
        /// Allow to add inputs to the instruction
        /// </summary>
        /// <param name="inputs">Dictionnary of inputs</param>
        public void AddInputs(Dictionary<string, Entity.Variable> inputs)
        {
            foreach (KeyValuePair<string, Entity.Variable> input in inputs)
            {
                AddInput(input.Key, input.Value);
            }
        }

        /// <summary>
        /// Allow to add an output to the instruction
        /// </summary>
        /// <param name="name">Name of the output</param>
        /// <param name="definition">Variable definition of the output</param>
        public void AddOutput(string name, Entity.Variable definition)
        {
            this.outputs[name] = new Output(definition);
        }

        /// <summary>
        /// Allow to add outputs to the instruction
        /// </summary>
        /// <param name="outputs">Dictionnary of outputs to set</param>
        public void AddOutputs(Dictionary<string, Entity.Variable> outputs)
        {
            foreach (KeyValuePair<string, Entity.Variable> output in outputs)
            {
                AddOutput(output.Key, output.Value);
            }
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
            if (!inputs.ContainsKey(name))
                throw new Error.NotFoundException("No such input named: " + name);
            inputs[name].Value = value;
        }

        /// <summary>
        /// Allow to get the value of a specific input
        /// </summary>
        /// <param name="name">Name of the input from which retreive the value</param>
        /// <returns>Value of the input</returns>
        public dynamic GetInputValue(string name)
        {
            return GetInput(name).Value;
        }
        
        /// <summary>
        /// Set the value of a specific output
        /// Protected because only an instruction can set the value of its output
        /// </summary>
        /// <param name="name">Name of the output to set the value</param>
        /// <param name="value">Value to set to the output</param>
        protected void SetOutputValue(string name, dynamic value)
        {
            if (!outputs.ContainsKey(name))
                throw new Error.NotFoundException("No such output named: " + name);
            outputs[name].Value = value;
        }

        /// <summary>
        /// Allow to get the value of a specific output
        /// </summary>
        /// <param name="name">Name of the output from which find the value</param>
        /// <returns>Desired output value</returns>
        public dynamic GetOutputValue(string name)
        {
            return GetOutput(name).Value;
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