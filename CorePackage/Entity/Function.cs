using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            if (this.parameters.Keys.Contains(name))
                this.parameters[name].definition.Value = value;
            else
                throw new KeyNotFoundException("Function: No such parameter named \"" + name + "\"");
        }

        /// <summary>
        /// Allow user to get the parameters that corresponds to the given name
        /// Throws a KeyNotFoundException if doesn't exists
        /// </summary>
        /// <param name="name">Name of the parameter to find</param>
        /// <returns>Variable definition that corresponds to the parameter</returns>
        public Variable GetParameter(string name)
        {
            if (!this.parameters.ContainsKey(name))
                throw new KeyNotFoundException("Function: No such parameter named \"" + name + "\"");
            return this.parameters[name].definition;
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
        public Variable GetReturn(string name)
        {
            if (!this.returns.ContainsKey(name))
                throw new KeyNotFoundException("Function: No such return named \"" + name + "\"");
            return this.returns[name].definition;
        }

        /// <summary>
        /// Execute internals instructions
        /// </summary>
        public void Call()
        {
            Stack<Execution.ExecutionRefreshInstruction> instructions = new Stack<Execution.ExecutionRefreshInstruction>();

            if (entrypoint == null)
                throw new InvalidOperationException("Function entry point has not been defined yet");
            instructions.Push(entrypoint);
            while (instructions.Count > 0)
            {
                Execution.ExecutionRefreshInstruction toexecute = instructions.Pop();
                
                toexecute.Execute();

                Execution.ExecutionRefreshInstruction[] nexts = toexecute.GetNextInstructions();

                foreach (Execution.ExecutionRefreshInstruction curr in nexts)
                {
                    if (curr != null)
                        instructions.Push(curr);
                }
            }
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used to convert the function into dot file. Will declare a node and recursively declare it's linked inputs
        /// </summary>
        /// <param name="node">The node to declare</param>
        /// <param name="id">Current node identifier that will be incremented</param>
        /// <param name="declared">Dictionarry of already declared node</param>
        /// <returns>The dot file lines to add to the file</returns>
        private string DeclareNode(Execution.Instruction node, ref int id, Dictionary<Execution.Instruction, string> declared)
        {
            //name of the node to declare
            string name = "node_" + id.ToString();
            id++;
            declared[node] = name;

            //process its inputs
            if (node.Inputs.Count > 0)
            {
                //contains inputs declaration
                string decl = "";
                //containes inputs links
                string links = "";

                //node input unique identifier
                int inputId = 0;
                //concatenation of declared name to make splitted node
                string inputs = "";

                //resolve inputs declaration
                foreach (Execution.Input curr in node.Inputs)
                {
                    //input name that depends on node name
                    string inpName = name + "_var_" + inputId.ToString();
                    //label of the input in order to be able to link it
                    string label = "<" + inpName + "> " + curr.Value.name + (curr.LinkedInstruction == null ? " = " + curr.Value.definition.Value.ToString() : "");

                    ++inputId;
                    //concatenate label to inputs for splitted box effect
                    inputs += label + (inputId < node.Inputs.Count ? "|" : "");

                    if (curr.LinkedInstruction == null)
                        continue;

                    //in case there is a linked node to the input, declare it
                    if (!declared.ContainsKey(curr.LinkedInstruction))
                        decl += DeclareNode(curr.LinkedInstruction, ref id, declared);

                    //link this node to the labeled input
                    links += declared[curr.LinkedInstruction] + " -> " + name + ":" + inpName + " [style=dotted;label=\"" + curr.LinkedOutputName + "\"];\r\n";
                }

                //splitted box format with each inputs labeled and linked to their node
                return decl + name + " [shape=record,label=\"{" + inputs + "}|<" + name + "_exec> " + node.GetType().ToString().Split('.').Last() + "\",color=" + (typeof(Execution.ExecutionRefreshInstruction).IsAssignableFrom(node.GetType()) ? "red" : "blue") + "];\r\n" + links;
            }
            //basic node, circle in red or blue
            return name + " [label=\"" + node.GetType().ToString().Split('.').Last() + "\",color=" + (typeof(Execution.ExecutionRefreshInstruction).IsAssignableFrom(node.GetType()) ? "red" : "blue") + "];\r\n";
        }

        /// <summary>
        /// Converts a function into a dot file
        /// </summary>
        /// <returns>The dot data to write into a file</returns>
        public string ToDotFile()
        {
            //unique identifier for a declared node
            int node_id = 0;

            //stack of graph instructions => for graph exploration
            Stack<Execution.ExecutionRefreshInstruction> instr = new Stack<Execution.ExecutionRefreshInstruction>();

            //Dictionarry of declared nodes that associates a reference of an instruction to its name in the dot file
            Dictionary<Execution.Instruction, string> declared = new Dictionary<Execution.Instruction, string>();

            //data that will contain dot file text and that will be returned
            string text = "digraph G {\r\n";
            
            instr.Push(entrypoint);
            while (instr.Count > 0)
            {
                //current instruction to process
                Execution.ExecutionRefreshInstruction toprocess = instr.Pop();

                //Checks if the instruction need to be declared
                if (!declared.ContainsKey(toprocess))
                    text += DeclareNode(toprocess, ref node_id, declared);

                //current instruction declaration name
                string decname = declared[toprocess];
                
                //Add each instruction linked to the current one
                foreach (Execution.ExecutionRefreshInstruction curr in toprocess.OutPoints)
                {
                    if (curr == null)
                        continue;

                    //declare it if needed
                    if (!declared.ContainsKey(curr))
                    {
                        text += DeclareNode(curr, ref node_id, declared);

                        //in case the instruction is not declared, push it in the stack to process it
                        instr.Push(curr);
                    }

                    //add the link between current node and its linked one
                    text += decname + ":" + decname + "_exec -> " + declared[curr] + ":" + declared[curr] + "_exec [color=red];\r\n";
                }
            }

            //end the file
            text += "}";

            return text;
        }
    }
}
