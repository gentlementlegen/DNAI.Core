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
                    this.inputs[curr.Key] = new Input(scope.AddExternal(curr.Key, curr.Value));
                }

            if (outputs != null)
                foreach (KeyValuePair<string, Entity.Variable> curr in outputs)
                {
                    this.outputs[curr.Key] = new Output(scope.AddExternal(curr.Key, curr.Value));
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

        public Input GetInput(string name)
        {
            return this.inputs[name];
        }

        public virtual Output GetOutput(string name)
        {
            return this.outputs[name];
        }

        public abstract void Execute();
    }
}
