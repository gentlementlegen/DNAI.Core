using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Global
{
    /// <summary>
    /// Represent a class to declare and define items
    /// </summary>
    public class Declarator<DefinitionType> : IDeclarator<DefinitionType>
    {
        private struct Declaration
        {
            public DefinitionType definition;
            public AccessMode visibility;
        };

        /// <summary>
        /// Internal list of defined items referenced by <c>externals</c> and <c>internal</c>
        /// declarations
        /// </summary>
        private Dictionary<string, Declaration> defined = new Dictionary<string, Declaration>();
        
        private Declaration _find(string name)
        {
            if (!defined.ContainsKey(name))
                throw new KeyNotFoundException("No such \"" + name + "\" in declarator");
            return defined[name];
        }

        public DefinitionType ChangeVisibility(string name, AccessMode newVisibility)
        {
            Declaration tochange = _find(name);

            tochange.visibility = newVisibility;
            return tochange.definition;
        }

        public DefinitionType Declare(DefinitionType entity, string name, AccessMode visibility)
        {
            if (defined.ContainsKey(name))
                throw new InvalidOperationException("Declarator.Declare : trying to redeclare \"" + name + "\"");

            defined[name] = new Declaration { definition = entity, visibility = visibility };

            return entity;
        }

        public DefinitionType Find(string name, AccessMode visibility)
        {
            Declaration toret = _find(name);

            if (toret.visibility != visibility)
                throw new InvalidOperationException("Declarator.Find : \"" + name + "\" has been found but visibility wasn't same");
            return toret.definition;
        }

        public DefinitionType Find(string name)
        {
            return _find(name).definition;
        }

        public DefinitionType Pop(string name)
        {
            Declaration topop = _find(name);

            defined.Remove(name);
            return topop.definition;
        }

        public DefinitionType Rename(string lastName, string newName)
        {
            if (defined.ContainsKey(newName))
                throw new InvalidOperationException("Declarator.Rename : \"" + newName + "\" already exists in declarator");

            Declaration torename = _find(lastName);

            defined.Remove(lastName);
            defined[newName] = torename;
            return torename.definition;
        }

        public DefinitionType GetVisibilityOf(string name, ref AccessMode visibility)
        {
            Declaration toret = _find(name);
            visibility = toret.visibility;
            return toret.definition;
        }
    }
}



/*/// <summary>
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
    this.internals[name] = new Declaration<DefinitionType> { name = name, definition = definition };
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
public Declaration<DefinitionType> FindInternalFrom(string name)
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
}*/