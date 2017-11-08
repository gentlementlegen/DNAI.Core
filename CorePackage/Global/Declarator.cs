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
        /// <summary>
        /// Class that associates a definition to its visibility
        /// </summary>
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
        public DefinitionType ChangeVisibility(string name, AccessMode newVisibility)
        {
            Declaration tochange = _find(name);

            tochange.visibility = newVisibility;
            return tochange.definition;
        }

        ///<see cref="IDeclarator{definitionType}.Declare(definitionType, string, AccessMode)"/>
        public DefinitionType Declare(DefinitionType entity, string name, AccessMode visibility)
        {
            if (defined.ContainsKey(name))
                throw new InvalidOperationException("Declarator.Declare : trying to redeclare \"" + name + "\"");

            defined[name] = new Declaration { definition = entity, visibility = visibility };

            return entity;
        }

        ///<see cref="IDeclarator{definitionType}.Find(string, AccessMode)"/>
        public DefinitionType Find(string name, AccessMode visibility)
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
        public DefinitionType Find(string name)
        {
            return _find(name).definition;
        }

        ///<see cref="IDeclarator{definitionType}.Pop(string)"/>
        public DefinitionType Pop(string name)
        {
            Declaration topop = _find(name);

            defined.Remove(name);
            return topop.definition;
        }

        ///<see cref="IDeclarator{definitionType}.Rename(string, string)"/>
        public DefinitionType Rename(string lastName, string newName)
        {
            if (defined.ContainsKey(newName))
                throw new InvalidOperationException("Declarator.Rename : \"" + newName + "\" already exists in declarator");

            Declaration torename = _find(lastName);

            defined.Remove(lastName);
            defined[newName] = torename;
            return torename.definition;
        }

        ///<see cref="IDeclarator{definitionType}.GetVisibilityOf(string, ref AccessMode)"/>
        public DefinitionType GetVisibilityOf(string name, ref AccessMode visibility)
        {
            Declaration toret = _find(name);
            visibility = toret.visibility;
            return toret.definition;
        }

        ///<see cref="IDeclarator{definitionType}.Clear"/>
        public List<DefinitionType> Clear()
        {
            List<DefinitionType> to_ret = new List<DefinitionType>();

            foreach (Declaration curr in defined.Values)
            {
                to_ret.Add(curr.definition);
            }
            defined.Clear();
            return to_ret;
        }
    }
}