using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StorageDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Variable>;

namespace CorePackage.Entity
{
    /// <summary>
    /// Class that represents a function definition
    /// </summary>
    public class Function : Global.Definition
    {
        /// <summary>
        /// Enumeration that represents function variables role
        /// Externals are split in PARAMETER and RETURN
        /// Internals are appart
        /// </summary>
        public enum VariableRole
        {
            PARAMETER,
            RETURN,
            INTERNAL
        }

        /// <summary>
        /// A function has an internal scope in which you can declare variables
        /// </summary>
        private StorageDeclarator scope = new StorageDeclarator();

        /// <summary>
        /// Contains function parameters which references variables declared in "scope" attribute
        /// </summary>
        private Dictionary<string, Global.Declaration<Variable>> parameters = new Dictionary<string, Global.Declaration<Variable>>();

        /// <summary>
        /// Contains function returns which references variables declared in "scope" attribute
        /// </summary>
        private Dictionary<string, Global.Declaration<Variable>> returns = new Dictionary<string, Global.Declaration<Variable>>();

        /// <summary>
        /// Contained instructions to process
        /// </summary>
        public List<Execution.Instruction> instructions = new List<Execution.Instruction>();

        /// <summary>
        /// First instruction to execute. Reference an instruction from <c>instructions</c> parameter
        /// </summary>
        public Execution.ExecutionRefreshInstruction entrypoint;

        /// <summary>
        /// Allow user to add a new variable in the function
        /// </summary>
        /// <param name="name">Name of the declared variable</param>
        /// <param name="definition">Definition of the variable</param>
        /// <param name="role">Variable role in the function</param>
        /// <returns>Declaration of the variable</returns>
        public Global.Declaration<Variable>  AddVariable(string name, Entity.Variable definition, VariableRole role)
        {
            Global.AccessMode mode = (role == VariableRole.INTERNAL ? Global.AccessMode.INTERNAL : Global.AccessMode.EXTERNAL);
            Global.Declaration<Variable> variable = this.scope.Add(name, definition, mode);
            if (role == VariableRole.PARAMETER)
                this.parameters[name] = variable;
            else if (role == VariableRole.RETURN)
                this.returns[name] = variable;
            return variable;
        }

        /// <summary>
        /// Make paremeters attributes publics in read only
        /// </summary>
        public List<Global.Declaration<Variable>> Parameters
        {
            get { return this.parameters.Values.ToList(); }
        }

        /// <summary>
        /// Allow user to set a parameter value from its name
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value to set</param>
        public void SetParameterValue(string name, dynamic value)
        {
            /*if (this.parameters.Keys.Contains(name))
                this.parameters[name].definition.Set(value);*/
        }

        /// <summary>
        /// Make returns attributes public in read only
        /// </summary>
        public List<Global.Declaration<Variable>> Returns
        {
            get { return this.returns.Values.ToList(); }
        }

        /// <summary>
        /// Allow to get a return value from its name
        /// </summary>
        /// <param name="name">Name of the return</param>
        /// <returns>Value to find or null</returns>
        public dynamic GetReturnValue(string name)
        {
            /*if (this.returns.Keys.Contains(name))
                return this.returns[name].definition.value;*/
            return 0;
        }

        /// <summary>
        /// Execute internals instructions
        /// </summary>
        public void Call()
        {
            //execution algorithm
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
