using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ContextDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Context>;
using StorageDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Variable>;
using TypesDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.DataType>;
using MethodsDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Function>;

namespace CorePackage.Entity
{
    /// <summary>
    /// A context is an entity in which you can declare variables, types, functions and other contexts
    /// </summary>
    public class Context : Global.Definition
    {
        /// <summary>
        /// A reference on its parent create a contexte access network
        /// </summary>
        private Context parent;

        /// <summary>
        /// Each context knows who are their children to be able
        /// to retreive children's externals items 
        /// </summary>
        private ContextDeclarator children;

        /// <summary>
        /// Declare and define variables
        /// </summary>
        private StorageDeclarator storage;

        /// <summary>
        /// Declare and define nested types
        /// </summary>
        private TypesDeclarator types;

        /// <summary>
        /// Declare and define methods
        /// </summary>
        private MethodsDeclarator methods;

        /// <summary>
        /// Constructor ask for its parent context
        /// </summary>
        /// <param name="parent">Parent context of the new one</param>
        public Context(Context parent = null)
        {
            this.parent = parent;
            this.children = new ContextDeclarator();
            this.storage = new StorageDeclarator();
            this.types = new TypesDeclarator();
            this.methods = new MethodsDeclarator();
        }

        /// <summary>
        /// Allow user to declare a new context
        /// </summary>
        /// <param name="name">Represents the name of the declared context</param>
        /// <param name="definition">Represents the definition of the context</param>
        /// <param name="mode">Represents the access mode of the context</param>
        /// <returns>Declaration of the new context</returns>
        public Global.Declaration<Context> DeclareNewContext(string name, Context definition = null, Global.AccessMode mode = Global.AccessMode.EXTERNAL)
        {
            if (definition != null)
                definition = new Context(this);
            return this.children.Add(name, definition, mode);
        }

        /// <summary>
        /// Allow user to declare a new function
        /// </summary>
        /// <param name="name">Represents the name of the declared function</param>
        /// <param name="definition">Represents the definition of the function</param>
        /// <param name="mode">Represents the access mode of the function</param>
        /// <returns>Declaration of the new function</returns>
        public Global.Declaration<Function> DeclareNewMethod(string name, Function definition = null, Global.AccessMode mode = Global.AccessMode.EXTERNAL)
        {
            if (definition != null)
                definition = new Function();
            return this.methods.Add(name, definition, mode);
        }

        /// <summary>
        /// Allow user to declare a new type
        /// </summary>
        /// <param name="name">Represents the name of the declared type</param>
        /// <param name="definition">Represents the definition of the type</param>
        /// <param name="mode">Represents the access mode of the type</param>
        /// <returns>Declaration of the new type</returns>
        public Global.Declaration<DataType> DeclareNewType(string name, DataType definition, Global.AccessMode mode = Global.AccessMode.EXTERNAL)
        {
            if (definition.GetType() == typeof(Type.ObjectType))
                this.DeclareNewContext(name, (definition as Type.ObjectType).Context, mode);
            return this.types.Add(name, definition, mode);
        }

        /// <summary>
        /// Allow user to declare a new variable
        /// </summary>
        /// <param name="name">Represents the name of the declarated variable</param>
        /// <param name="definition">Represents the definition of the type</param>
        /// <param name="mode">Represents the access mode of the type</param>
        /// <returns>Declaration of the new variable</returns>
        public Global.Declaration<Variable> DeclareNewVariable(string name, Variable definition, Global.AccessMode mode = Global.AccessMode.EXTERNAL)
        {
            return this.storage.Add(name, definition, mode);
        }

        /// <summary>
        /// Allow user to find a context from its name
        /// </summary>
        /// <param name="name">Name of the context to find</param>
        /// <returns>Context associated to the given name or null</returns>
        public Global.Declaration<Context> FindContextFrom(string name)
        {
            return this.children.FindFrom(name);
        }

        /// <summary>
        /// Allow user to find a function from its name
        /// </summary>
        /// <param name="name">Name of the function to find</param>
        /// <returns>Function associated to the given name or null</returns>
        public Global.Declaration<Function> FindMethodFrom(string name)
        {
            return this.methods.FindFrom(name);
        }

        /// <summary>
        /// Allow user to find a type from its name
        /// </summary>
        /// <param name="name">Name of the type to find</param>
        /// <returns>Type associated to the given name or null</returns>
        public Global.Declaration<DataType> FindTypeFrom(string name)
        {
            return this.types.FindFrom(name);
        }

        /// <summary>
        /// Allow user to find a variable from its name
        /// </summary>
        /// <param name="name">Nmae of the variable to find</param>
        /// <returns>Variable associated to the given name or null</returns>
        public Global.Declaration<Variable> FindVariableFrom(string name, Global.AccessMode access = Global.AccessMode.EXTERNAL)
        {
            return this.storage.FindFrom(name, access);
        }

        /// <see cref="Global.Definition"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
