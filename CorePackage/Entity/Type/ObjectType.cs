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
    public class ObjectType : DataType
    {
        /// <summary>
        /// The context of an object in which you can declare method, static attributes, nested types and other contexts
        /// </summary>
        private Context context;

        /// <summary>
        /// Used to represent object public attributes 
        /// </summary>
        private Dictionary<string, DataType> publicAttributes;

        /// <summary>
        /// Used to represent object private attributes
        /// </summary>
        private Dictionary<string, DataType> privateAttributes;

        /// <summary>
        /// Constructor that asks for the object parent context in order to link his internal context
        /// </summary>
        /// <param name="parent">Parent context of the object</param>
        public ObjectType(Context parent)
        {
            this.context = new Context(parent);
            publicAttributes = new Dictionary<string, DataType>();
            privateAttributes = new Dictionary<string, DataType>();
        }

        /// <summary>
        /// Make internal context visible in read only
        /// </summary>
        public Context Context
        {
            get { return context; }
        }

        /// <summary>
        /// Allow to add a public attribute to the object
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="definition">Variable definition of the attribute</param>
        /// <returns>Freshly created declaration from name and definition</returns>
        public void AddPublicAttribute(string name, DataType attrType)
        {
            this.publicAttributes[name] = attrType;
        }

        /// <summary>
        /// Allow to add a private attribute to the object
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="definition">Variable definition of the attribute</param>
        /// <returns>Freshly created declaration from name and definition</returns>
        public void AddPrivateAttribute(string name, DataType attrType)
        {
            this.privateAttributes[name] = attrType;
        }

        Dictionary<string, DataType> PublicAttributes
        {
            get { return publicAttributes; }
        }

        Dictionary<string, DataType> PrivateAttributes
        {
            get { return privateAttributes; }
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
    }
}
