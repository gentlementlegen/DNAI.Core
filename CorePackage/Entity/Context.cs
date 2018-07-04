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
    public class Context : Definition, IDeclarator
    {
        /// <summary>
        /// Each context knows who are their children to be able
        /// to retreive children's externals items 
        /// </summary>
        private Declarator scope = new Declarator(new List<System.Type> { typeof(Context), typeof(Variable), typeof(Function), typeof(DataType) });
        
        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
        public Context()
        {

        }

        /// <see cref="Global.IDefinition"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        public void ChangeVisibility(string name, AccessMode newVisibility)
        {
            scope.ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public IDefinition Declare(IDefinition entity, string name, AccessMode visibility)
        {
            return scope.Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        public IDefinition Find(string name, AccessMode visibility)
        {
            return scope.Find(name, visibility);
        }

        /// <see cref="IDeclarator.Find(string)"/>
        public IDefinition Find(string name)
        {
            return scope.Find(name);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        public IDefinition Pop(string name)
        {
            return scope.Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        public void Rename(string lastName, string newName)
        {
            scope.Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        public AccessMode GetVisibilityOf(string name)
        {
            return scope.GetVisibilityOf(name);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<IDefinition> Clear()
        {
            return scope.Clear();
        }


        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        public Dictionary<string, IDefinition> GetEntities(AccessMode visibility)
        {
            return scope.GetEntities(visibility);
        }

        ///<see cref="IDeclarator.GetEntities()"/>
        public Dictionary<string, IDefinition> GetEntities()
        {
            return scope.GetEntities();
        }

        public bool Contains(string name)
        {
            return scope.Contains(name);
        }
    }
}
