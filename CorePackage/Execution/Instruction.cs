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

        public List<Input> Inputs
        {
            get { return inputs.Values.ToList(); }
        }

        public List<Output> Outputs
        {
            get { return outputs.Values.ToList(); }
        }

        public void AddInput(string name, Entity.Variable definition)
        {
            AddInput(scope.AddExternal(name, definition));
        }

        public void AddInput(Global.Declaration<Entity.Variable> declaration)
        {
            this.inputs[declaration.name] = new Input(declaration);
        }

        public void AddOutput(string name, Entity.Variable definition)
        {
            AddOutput(scope.AddExternal(name, definition));
        }

        public void AddOutput(Global.Declaration<Entity.Variable> declaration)
        {
            this.outputs[declaration.name] = new Output(declaration);
        }

        public Input GetInput(string name)
        {
            if (!this.inputs.ContainsKey(name))
                throw new KeyNotFoundException("No such input named: " + name);
            return this.inputs[name];
        }

        public virtual Output GetOutput(string name)
        {
            if (!this.outputs.ContainsKey(name))
                throw new KeyNotFoundException("No such output named: " + name);
            return this.outputs[name];
        }

        public void SetInputValue(string name, dynamic value)
        {
            GetInput(name).Value.definition.Value = value;
        }

        public dynamic GetInputValue(string name)
        {
            return this.GetInput(name).Value.definition.Value;
        }

        public dynamic GetOutputValue(string name)
        {
            return this.GetOutput(name).Value.definition.Value;
        }

        public abstract void Execute();
    }
}
