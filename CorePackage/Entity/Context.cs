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
    public class Context : Definition, IContext
    {
        /// <summary>
        /// A reference on its parent create a contexte access network
        /// </summary>
        private IContext parent;

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
        /// Constructor ask for its parent context
        /// </summary>
        /// <param name="parent">Parent context of the new one</param>
        public Context(IContext parent = null)
        {
            this.parent = parent;
        }

        /// <see cref="Global.Definition"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public void SetParent(IContext parent)
        {
            this.parent = parent;
        }

        IContext IDeclarator<IContext>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return children.ChangeVisibility(name, newVisibility);
        }

        IContext IDeclarator<IContext>.Declare(IContext entity, string name, AccessMode visibility)
        {
            return children.Declare(entity, name, visibility);
        }

        IContext IDeclarator<IContext>.Find(string name, AccessMode visibility)
        {
            return children.Find(name, visibility);
        }

        IContext IDeclarator<IContext>.Pop(string name)
        {
            return children.Pop(name);
        }

        IContext IDeclarator<IContext>.Rename(string lastName, string newName)
        {
            return children.Rename(lastName, newName);
        }

        IContext IDeclarator<IContext>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return children.GetVisibilityOf(name, ref visibility);
        }

        Variable IDeclarator<Variable>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return storage.ChangeVisibility(name, newVisibility);
        }

        Variable IDeclarator<Variable>.Declare(Variable entity, string name, AccessMode visibility)
        {
            return storage.Declare(entity, name, visibility);
        }

        Variable IDeclarator<Variable>.Find(string name, AccessMode visibility)
        {
            return storage.Find(name, visibility);
        }

        Variable IDeclarator<Variable>.Pop(string name)
        {
            return storage.Pop(name);
        }

        Variable IDeclarator<Variable>.Rename(string lastName, string newName)
        {
            return storage.Rename(lastName, newName);
        }
        
        Variable IDeclarator<Variable>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return storage.GetVisibilityOf(name, ref visibility);
        }

        DataType IDeclarator<DataType>.Declare(DataType entity, string name, AccessMode visibility)
        {
            return types.Declare(entity, name, visibility);
        }

        DataType IDeclarator<DataType>.Pop(string name)
        {
            return types.Pop(name);
        }

        DataType IDeclarator<DataType>.Find(string name, AccessMode visibility)
        {
            return types.Find(name, visibility);
        }

        DataType IDeclarator<DataType>.Rename(string lastName, string newName)
        {
            return types.Rename(lastName, newName);
        }

        DataType IDeclarator<DataType>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return types.ChangeVisibility(name, newVisibility);
        }
        DataType IDeclarator<DataType>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return types.GetVisibilityOf(name, ref visibility);
        }

        Function IDeclarator<Function>.Declare(Function entity, string name, AccessMode visibility)
        {
            return methods.Declare(entity, name, visibility);
        }

        Function IDeclarator<Function>.Pop(string name)
        {
            return methods.Pop(name);
        }

        Function IDeclarator<Function>.Find(string name, AccessMode visibility)
        {
            return methods.Find(name, visibility);
        }

        Function IDeclarator<Function>.Rename(string lastName, string newName)
        {
            return methods.Rename(lastName, newName);
        }

        Function IDeclarator<Function>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return methods.ChangeVisibility(name, newVisibility);
        }

        Function IDeclarator<Function>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return methods.GetVisibilityOf(name, ref visibility);
        }
    }
}
