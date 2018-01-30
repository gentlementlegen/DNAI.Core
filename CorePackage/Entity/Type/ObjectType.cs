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

        /// <summary>
        /// Represents the objects attributes through a declarator of DataType
        /// </summary>
        private Declarator<DataType> attributes = new Declarator<DataType>();

        /// <summary>
        /// Basic default constructor which is necessary for factory
        /// </summary>
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
        
        /// <summary>
        /// Allow user to add an attribute with a name, a type and a visibility
        /// </summary>
        /// <param name="name">Name of the attribute to add</param>
        /// <param name="attrType">Type of the attribute to add</param>
        /// <param name="visibility">Visibility of the attribute (INTERNAL, EXTERNAL)</param>
        public void AddAttribute(string name, DataType attrType, Global.AccessMode visibility)
        {
            attributes.Declare(attrType, name, visibility);
        }

        /// <summary>
        /// Allow user to remove an internal attribute with a specific name
        /// </summary>
        /// <param name="name">Name of the attribute to remove</param>
        public void RemoveAttribute(string name)
        {
            attributes.Pop(name);
        }

        /// <summary>
        /// Allow user to change an attribute visibility
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="visibility">Visibility to set</param>
        public void ChangeAttributeVisibility(string name, Global.AccessMode visibility)
        {
            attributes.ChangeVisibility(name, visibility);
        }

        /// <summary>
        /// Allow user to rename an attribute
        /// </summary>
        /// <param name="lastName">Current name of the attribute</param>
        /// <param name="newName">New name to set to the attribute</param>
        public void RenameAttribute(string lastName, string newName)
        {
            attributes.Rename(lastName, newName);
        }

        public void SetFunctionAsMember(string name, Global.AccessMode visibility)
        {
            Function func = ((IDeclarator<Function>)this).Find(name, visibility);

            if (func == null)
                return;

            func.Declare(new Variable(this), "this", AccessMode.EXTERNAL);
            func.SetVariableAs("this", Function.VariableRole.PARAMETER);
        }

        /// <see cref="DataType.Instantiate"/>
        public override dynamic Instantiate()
        {
            Dictionary<string, dynamic> data = new Dictionary<string, dynamic>();

            foreach(KeyValuePair<string, DataType> attrtype in attributes.GetEntities(AccessMode.EXTERNAL))
            {
                data[attrtype.Key] = attrtype.Value.Instantiate();
            }
            foreach (KeyValuePair<string, DataType> attrtype in attributes.GetEntities(AccessMode.INTERNAL))
            {
                data[attrtype.Key] = attrtype.Value.Instantiate();
            }
            return data;
        }

        /// <see cref="DataType.IsValueOfType(dynamic)"/>
        public override bool IsValueOfType(dynamic value)
        {
            foreach (KeyValuePair<string, DataType> attrtype in attributes.GetEntities(AccessMode.EXTERNAL))
            {
                if (!value.ContainsKey(attrtype.Key)
                    || !attrtype.Value.IsValueOfType(value[attrtype.Key]))
                    return false;
            }
            
            foreach (KeyValuePair<string, DataType> attrtype in attributes.GetEntities(AccessMode.INTERNAL))
            {
                if (!value.ContainsKey(attrtype.Key)
                    || !attrtype.Value.IsValueOfType(value[attrtype.Key]))
                    return false;
            }
            return true;
        }

        /// <see cref="Global.Definition.IsValid"/>
        public override bool IsValid()
        {
            throw new NotImplementedException();
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public IContext Declare(IContext entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        public IContext Pop(string name)
        {
            return ((IDeclarator<IContext>)context).Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        public IContext Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        public IContext Rename(string lastName, string newName)
        {
            return ((IDeclarator<IContext>)context).Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        public IContext GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        public IContext ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<IContext>)context).ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<IContext> Clear()
        {
            return ((IDeclarator<IContext>)context).Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, IContext> IDeclarator<IContext>.GetEntities(AccessMode visibility)
        {
            return ((IDeclarator<IContext>)context).GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public Variable Declare(Variable entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        Variable IDeclarator<Variable>.Pop(string name)
        {
            return ((IDeclarator<Variable>)context).Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        Variable IDeclarator<Variable>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        Variable IDeclarator<Variable>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<Variable>)context).Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        Variable IDeclarator<Variable>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        Variable IDeclarator<Variable>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<Variable>)context).ChangeVisibility(name, newVisibility);
        }
        
        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<Variable> IDeclarator<Variable>.Clear()
        {
            return ((IDeclarator<Variable>)context).Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, Variable> IDeclarator<Variable>.GetEntities(AccessMode visibility)
        {
            return ((IDeclarator<Variable>)context).GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public DataType Declare(DataType entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        DataType IDeclarator<DataType>.Pop(string name)
        {
            return ((IDeclarator<DataType>)context).Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        DataType IDeclarator<DataType>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).Find(name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        DataType IDeclarator<DataType>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<DataType>)context).Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        DataType IDeclarator<DataType>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        DataType IDeclarator<DataType>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<DataType>)context).ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<DataType> IDeclarator<DataType>.Clear()
        {
            return ((IDeclarator<DataType>)context).Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, DataType> IDeclarator<DataType>.GetEntities(AccessMode visibility)
        {
            return ((IDeclarator<DataType>)context).GetEntities(visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public Function Declare(Function entity, string name, AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).Declare(entity, name, visibility);
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        Function IDeclarator<Function>.Pop(string name)
        {
            return ((IDeclarator<Function>)context).Pop(name);
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        Function IDeclarator<Function>.Find(string name, AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).Find(name, visibility);
        }
        
        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        Function IDeclarator<Function>.Rename(string lastName, string newName)
        {
            return ((IDeclarator<Function>)context).Rename(lastName, newName);
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        Function IDeclarator<Function>.GetVisibilityOf(string name, ref AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).GetVisibilityOf(name, ref visibility);
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        Function IDeclarator<Function>.ChangeVisibility(string name, AccessMode newVisibility)
        {
            return ((IDeclarator<Function>)context).ChangeVisibility(name, newVisibility);
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        List<Function> IDeclarator<Function>.Clear()
        {
            return ((IDeclarator<Function>)context).Clear();
        }

        ///<see cref="IDeclarator{definitionType}.GetEntities(AccessMode)"/>
        Dictionary<string, Function> IDeclarator<Function>.GetEntities(AccessMode visibility)
        {
            return ((IDeclarator<Function>)context).GetEntities(visibility);
        }

        /// <summary>
        /// Set parent context
        /// </summary>
        /// <param name="parent">Parent context to set</param>
        public void SetParent(IContext parent)
        {
            context.SetParent(parent);
        }

        public Dictionary<string, DataType> GetPublicAttributes()
        {
            return attributes.GetEntities(AccessMode.EXTERNAL);
        }
    }
}
