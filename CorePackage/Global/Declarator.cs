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
    public class Declarator : IDeclarator
    {
        /// <summary>
        /// Class that associates a definition to its visibility
        /// </summary>
        private class Declaration
        {
            public IDefinition definition;
            public AccessMode visibility;
        };

        private List<Type> handled = new List<Type>();

        /// <summary>
        /// Internal list of defined items referenced by <c>externals</c> and <c>internal</c>
        /// declarations
        /// </summary>
        private Dictionary<string, Declaration> defined = new Dictionary<string, Declaration>();
        
        public Declarator(List<Type> handled)
        {
            this.handled = handled;
        }

        /// <summary>
        /// Internal function to retreive an Entity from its name
        /// </summary>
        /// <remarks>Throws an Error.NotFoundException if not found</remarks>
        /// <param name="name">Name of the entity to find</param>
        /// <returns>Declaration which contains the entity and its visibility</returns>
        private Declaration _find(string name)
        {
            if (!defined.ContainsKey(name))
                throw new Error.NotFoundException("No such \"" + name + "\" in declarator");
            return defined[name];
        }

        ///<see cref="IDeclarator{definitionType}.ChangeVisibility(string, AccessMode)"/>
        public void ChangeVisibility(string name, AccessMode newVisibility)
        {
            Declaration tochange = _find(name);

            tochange.visibility = newVisibility;
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public IDefinition Declare(IDefinition entity, string name, AccessMode visibility)
        {
            if (defined.ContainsKey(name))
                throw new InvalidOperationException("Declarator.Declare : trying to redeclare \"" + name + "\"");

            System.Type entityType = entity.GetType();
            bool ok = false;

            foreach (System.Type curr in handled)
            {
                if (ok = curr.IsAssignableFrom(entityType))
                    break;
            }
            if (!ok)
                throw new InvalidOperationException("You cannot declare " + entity.GetType().ToString() + " in this container");

            defined[name] = new Declaration { definition = entity, visibility = visibility };

            return entity;
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        public IDefinition Find(string name, AccessMode visibility)
        {
            Declaration toret = _find(name);

            if (toret.visibility != visibility)
                throw new InvalidOperationException("Declarator.Find : \"" + name + "\" has been found but visibility wasn't same");
            return toret.definition;
        }

        /// <summary>
        /// Allow user to find an entity only with its name
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <returns>The retreived entity</returns>
        public IDefinition Find(string name)
        {
            return _find(name).definition;
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        public IDefinition Pop(string name)
        {
            Declaration topop = _find(name);

            defined.Remove(name);
            return topop.definition;
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        public void Rename(string lastName, string newName)
        {
            if (defined.ContainsKey(newName))
                throw new InvalidOperationException("Declarator.Rename : \"" + newName + "\" already exists in declarator");

            Declaration torename = _find(lastName);

            defined.Remove(lastName);
            defined[newName] = torename;
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        public AccessMode GetVisibilityOf(string name)
        {
            return _find(name).visibility;
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<IDefinition> Clear()
        {
            List<IDefinition> to_ret = new List<IDefinition>();

            foreach (Declaration curr in defined.Values)
            {
                to_ret.Add(curr.definition);
            }
            defined.Clear();
            return to_ret;
        }

        public Dictionary<string, IDefinition> GetEntities(AccessMode visibility)
        {
            Dictionary<string, IDefinition> toret = new Dictionary<string, IDefinition>();

            foreach (KeyValuePair<string, Declaration> curr in defined)
            {
                if (curr.Value.visibility == visibility)
                    toret.Add(curr.Key, curr.Value.definition);
            }
            return toret;
        }

        public Dictionary<string, IDefinition> GetEntities()
        {
            Dictionary<string, IDefinition> toret = new Dictionary<string, IDefinition>();

            foreach (KeyValuePair<string, Declaration> curr in defined)
            {
                toret.Add(curr.Key, curr.Value.definition);
            }
            return toret;
        }

        ///<see cref="IDeclarator.Contains(string)"/>
        public bool Contains(string name)
        {
            return defined.ContainsKey(name);
        }
    }
}