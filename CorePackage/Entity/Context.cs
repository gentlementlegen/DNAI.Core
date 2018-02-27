using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorePackage.Global;

namespace CorePackage.Entity
{
    /// <summary>
    /// A context is an entity in which you can declare variables, types, functions and other contexts
    /// </summary>
    public class Context : IContext
    {
        /// <summary>
        /// A reference on its parent create a contexte access network
        /// </summary>
        private IContext parent = null;

        /// <summary>
        /// Each context knows who are their children to be able
        /// to retreive children's externals items 
        /// </summary>
        private Declarator<IContext> children = new Declarator<IContext>();

        /// <summary>
        /// Declare and define variables
        /// </summary>
        private Declarator<Variable> storage = new Declarator<Variable>();

        /// <summary>
        /// Declare and define nested types
        /// </summary>
        private Declarator<DataType> types = new Declarator<DataType>();

        /// <summary>
        /// Declare and define methods
        /// </summary>
        private Declarator<Function> methods = new Declarator<Function>();

        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
        public Context()
        {

        }
        
        /// <summary>
        /// Constructor ask for its parent context
        /// </summary>
        /// <param name="parent">Parent context of the new one</param>
        public Context(IContext parent)
        {
            this.parent = parent;
        }

        /// <see cref="Global.Definition"/>
        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Allow to set parent context
        /// </summary>
        /// <param name="parent">parent context to set</param>
        public void SetParent(IContext parent)
        {
            this.parent = parent;
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        IContext IDeclarator<IContext>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return children.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        IContext IDeclarator<IContext>.Declare(IContext entity, string name, AccessMode visibility)
        {
            return children.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        IContext IDeclarator<IContext>.Find(string name, AccessMode visibility)
        {
            return children.Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        IContext IDeclarator<IContext>.Pop(string name)
        {
            return children.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        IContext IDeclarator<IContext>.Rename(string lastName, string newName)
        {
            return children.Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        IContext IDeclarator<IContext>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return children.GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<IContext> Clear()
        {
            return children.Clear();
        }


        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, IContext> IDeclarator<IContext>.GetEntities(AccessMode visibility)
        {
            return children.GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        Variable IDeclarator<Variable>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return storage.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        Variable IDeclarator<Variable>.Declare(Variable entity, string name, AccessMode visibility)
        {
            return storage.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        Variable IDeclarator<Variable>.Find(string name, AccessMode visibility)
        {
            return storage.Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        Variable IDeclarator<Variable>.Pop(string name)
        {
            return storage.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        Variable IDeclarator<Variable>.Rename(string lastName, string newName)
        {
            return storage.Rename(lastName, newName);
        }
        
        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        Variable IDeclarator<Variable>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return storage.GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<Variable> IDeclarator<Variable>.Clear()
        {
            return storage.Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, Variable> IDeclarator<Variable>.GetEntities(AccessMode visibility)
        {
            return storage.GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        DataType IDeclarator<DataType>.Declare(DataType entity, string name, AccessMode visibility)
        {
            return types.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        DataType IDeclarator<DataType>.Pop(string name)
        {
            return types.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        DataType IDeclarator<DataType>.Find(string name, AccessMode visibility)
        {
            return types.Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        DataType IDeclarator<DataType>.Rename(string lastName, string newName)
        {
            return types.Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        DataType IDeclarator<DataType>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return types.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        DataType IDeclarator<DataType>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return types.GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<DataType> IDeclarator<DataType>.Clear()
        {
            return types.Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, DataType> IDeclarator<DataType>.GetEntities(AccessMode visibility)
        {
            return types.GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        Function IDeclarator<Function>.Declare(Function entity, string name, AccessMode visibility)
        {
            return methods.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        Function IDeclarator<Function>.Pop(string name)
        {
            return methods.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        Function IDeclarator<Function>.Find(string name, AccessMode visibility)
        {
            return methods.Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        Function IDeclarator<Function>.Rename(string lastName, string newName)
        {
            return methods.Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        Function IDeclarator<Function>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return methods.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        Function IDeclarator<Function>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return methods.GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<Function> IDeclarator<Function>.Clear()
        {
            return methods.Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, Function> IDeclarator<Function>.GetEntities(AccessMode visibility)
        {
            return methods.GetEntities(visibility);
        }
    }
}
