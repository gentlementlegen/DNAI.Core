using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StorageDeclarator = CorePackage.Global.Declarator<CorePackage.Entity.Variable>;

namespace CorePackage.Entity.Type
{
    /// <summary>
    /// Represents a Object definition type
    /// </summary>
    public class ObjectType : DataType, IContext
    {
        /// <summary>
        /// The context of an object in which you can declare method, static attributes, nested types and other contexts
        /// </summary>
        private IContext context = new Context();

        private Declarator<DataType> attributes = new Declarator<DataType>();

        public ObjectType()
        {

        }

        /// <summary>
        /// Constructor that asks for the object parent context in order to link his internal context
        /// </summary>
        /// <param name="parent">Parent context of the object</param>
        public ObjectType(IContext parent)
        {
            this.context.SetParent(parent);
        }
        
        public void AddAttribute(string name, DataType attrType, Global.AccessMode visibility)
        {
            attributes.Declare(attrType, name, visibility);
        }

        public void RemoveAttribute(string name)
        {
            attributes.Pop(name);
        }

        public void ChangeAttributeVisibility(string name, Global.AccessMode visibility)
        {
            attributes.ChangeVisibility(name, visibility);
        }

        public void RenameAttribute(string lastName, string newName)
        {
            attributes.Rename(lastName, newName);
        }
                
        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            /*System.Dynamic.ExpandoObject toret = new System.Dynamic.ExpandoObject();

            foreach (Declaration.DeclarationNode attr in this.attributes.Internals)
            {
                toret[attr.name] = attr.definition.Instanciate();
            }*/
            throw new NotImplementedException();
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            throw new NotImplementedException();
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        public IContext Declare(IContext entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).Declare(entity, name, visibility);
        }

        public IContext Pop(string name)
        {
            return ((IDeclarator<IContext>)context).Pop(name);
        }

        public IContext Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).Find(name, visibility);
        }

        public IContext Rename(string lastName, string newName)
        {
            return ((IDeclarator<IContext>)context).Rename(lastName, newName);
        }

        public IContext GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).GetVisibilityOf(name, ref visibility);
        }

        public IContext ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<IContext>)context).ChangeVisibility(name, newVisibility);
        }

        public Variable Declare(Variable entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).Declare(entity, name, visibility);
        }

        Variable IDeclarator<Variable>.Pop(string name)
        {
            return ((IDeclarator<Variable>)context).Pop(name);
        }

        Variable IDeclarator<Variable>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).Find(name, visibility);
        }

        Variable IDeclarator<Variable>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<Variable>)context).Rename(lastName, newName);
        }

        Variable IDeclarator<Variable>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).GetVisibilityOf(name, ref visibility);
        }

        Variable IDeclarator<Variable>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<Variable>)context).ChangeVisibility(name, newVisibility);
        }

        public DataType Declare(DataType entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).Declare(entity, name, visibility);
        }

        DataType IDeclarator<DataType>.Pop(string name)
        {
            return ((IDeclarator<DataType>)context).Pop(name);
        }

        DataType IDeclarator<DataType>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).Find(name, visibility);
        }

        DataType IDeclarator<DataType>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<DataType>)context).Rename(lastName, newName);
        }

        DataType IDeclarator<DataType>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).GetVisibilityOf(name, ref visibility);
        }

        DataType IDeclarator<DataType>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<DataType>)context).ChangeVisibility(name, newVisibility);
        }

        public Function Declare(Function entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).Declare(entity, name, visibility);
        }

        Function IDeclarator<Function>.Pop(string name)
        {
            return ((IDeclarator<Function>)context).Pop(name);
        }

        Function IDeclarator<Function>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).Find(name, visibility);
        }

        Function IDeclarator<Function>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<Function>)context).Rename(lastName, newName);
        }

        Function IDeclarator<Function>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).GetVisibilityOf(name, ref visibility);
        }

        Function IDeclarator<Function>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<Function>)context).ChangeVisibility(name, newVisibility);
        }

        public void SetParent(IContext parent)
        {
            context.SetParent(parent);
        }
    }
}
