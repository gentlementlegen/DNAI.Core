using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    /// <summary>
    /// Enumeration that represents the access level of a declaration
    /// </summary>
    public enum AccessMode
    {
        INTERNAL,
        EXTERNAL
    };

    /// <summary>
    /// Represent a class to declare and define items
    /// </summary>
    public class Declarator<DefinitionType>
    {
        /// <summary>
        /// Internal list of defined items referenced by <c>externals</c> and <c>internal</c>
        /// declarations
        /// </summary>
        private List<DefinitionType> defined = new List<DefinitionType>();

        /// <summary>
        /// Declares externals variables that reference <c>defined</c> variables
        /// </summary>
        private Dictionary<string, Declaration<DefinitionType> > externals = new Dictionary<string, Declaration<DefinitionType>>();

        /// <summary>
        /// Declares internal variables that reference <c>defined</c> variables
        /// </summary>
        private Dictionary<string, Declaration<DefinitionType> > internals = new Dictionary<string, Declaration<DefinitionType>>();
        
        /// <summary>
        /// Allow user to declare a new entity in the declarator
        /// </summary>
        /// <param name="name">Name of the entity declared</param>
        /// <param name="definition">Definition of the entity</param>
        /// <param name="mode">Access mode of the entity</param>
        /// <returns>Declaration of the new entity or null</returns>
        public Declaration<DefinitionType> Add(string name, DefinitionType definition, AccessMode mode)
        {
            if (mode == AccessMode.INTERNAL)
                return this.AddInternal(name, definition);
            else if (mode == AccessMode.EXTERNAL)
                return this.AddExternal(name, definition);
            throw new ArgumentException("Declarator: access mode have to be INTERNAL or EXTERNAL");
        }

        /// <summary>
        /// Allow user to declare an internal entity
        /// </summary>
        /// <param name="name">Name of the declared entity</param>
        /// <param name="definition">Definition of the entity</param>
        /// <returns>Declaration of the entity</returns>
        public Declaration<DefinitionType> AddInternal(string name, DefinitionType definition)
        {
            this.internals[name] = new Declaration<DefinitionType>{ name = name, definition = definition};
            this.defined.Add(definition);
            return this.internals[name];
        }

        /// <summary>
        /// Allow user to declare an external entity
        /// </summary>
        /// <param name="name">Name of the declared entity</param>
        /// <param name="definition">Definition of the entity</param>
        /// <returns>Declaration of the entity</returns>
        public Declaration<DefinitionType> AddExternal(string name, DefinitionType definition)
        {
            Declaration<DefinitionType> tmp = new Declaration<DefinitionType> { name = name, definition = definition };
            this.externals[name] = tmp;
            this.defined.Add(definition);
            return tmp;
        }

        /// <summary>
        /// Allow user to find an entity from its name and access mode
        /// </summary>
        /// <param name="name">Name of the entity to find</param>
        /// <param name="mode">Access mode of the entity</param>
        /// <returns>Declaration of the found entity or null</returns>
        public Declaration<DefinitionType> FindFrom(string name, AccessMode mode = AccessMode.EXTERNAL)
        {
            if (mode == AccessMode.EXTERNAL)
                return this.FindExternalFrom(name);
            else if (mode == AccessMode.INTERNAL)
                return this.FindInternalFrom(name);
            throw new KeyNotFoundException("Declarator: No such entity named \"" + name + "\"");
        }

        /// <summary>
        /// Allow user to find an internal entity from its name
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <returns>Declaration of the entity</returns>
        public Declaration<DefinitionType>  FindInternalFrom(string name)
        {
            return this.internals[name];
        }
        
        /// <summary>
        /// Allow user to find an external entity from its name
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <returns>Declaration of the entity</returns>
        public Declaration<DefinitionType> FindExternalFrom(string name)
        {
            return this.externals[name];
        }

        /// <summary>
        /// Make internals attributes public in read only
        /// </summary>
        public List<Declaration<DefinitionType> > Internals
        {
            get { return this.internals.Values.ToList(); }
        }

        /// <summary>
        /// Make externals attributes public in read only
        /// </summary>
        public List<Declaration<DefinitionType> > Externals
        {
            get { return this.externals.Values.ToList(); }
        }
    }
}