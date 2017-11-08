using System;
using System.Collections.Generic;

namespace CoreControl
{
    /// <summary>
    /// Class that is used to manipulate entities
    /// </summary>
    public class EntityFactory
    {
        /// <summary>
        /// Ids of entities created by default
        /// </summary>
        public enum BASE_ID : uint
        {
            GLOBAL_CTX = 0,
            BOOLEAN_TYPE = 1,
            INTEGER_TYPE = 2,
            FLOATING_TYPE = 3,
            CHARACTER_TYPE = 4,
            STRING_TYPE = 5
        }

        /// <summary>
        /// Associates an id to its entity definition
        /// </summary>
        private Dictionary<UInt32, CorePackage.Global.Definition> definitions = new Dictionary<uint, CorePackage.Global.Definition>();

        /// <summary>
        /// Associates an entity definition to its id
        /// </summary>
        private Dictionary<CorePackage.Global.Definition, UInt32> ids = new Dictionary<CorePackage.Global.Definition, uint>();

        /// <summary>
        /// Represents the id of the next entity that will be declared
        /// </summary>
        private UInt32 current_uid = 0;
        
        /// <summary>
        /// Default constructor that add default entities (global context and scalar types)
        /// </summary>
        public EntityFactory()
        {
            //global context is in 0
            add_entity(new CorePackage.Entity.Context());

            //boolean type is in 1
            add_entity(CorePackage.Entity.Type.Scalar.Boolean);

            //integer type is in 2
            add_entity(CorePackage.Entity.Type.Scalar.Integer);

            //floating type is in 3
            add_entity(CorePackage.Entity.Type.Scalar.Floating);

            //character type is in 4
            add_entity(CorePackage.Entity.Type.Scalar.Character);

            //string type is in 5
            add_entity(CorePackage.Entity.Type.Scalar.String);
        }

        /// <summary>
        /// Getter for the current id
        /// </summary>
        public UInt32 CurrentID
        {
            get { return current_uid; }
        }

        /// <summary>
        /// Getter for the id of the last entity declared
        /// </summary>
        public UInt32 LastID
        {
            get { return CurrentID - 1; }
        }

        /// <summary>
        /// Creates an entity and add it the the dictionnaries
        /// </summary>
        /// <typeparam name="T">Type of the entity to declare</typeparam>
        /// <returns>Freshly instanciated entity</returns>
        public T create<T>() where T : CorePackage.Global.Definition
        {
            T toadd = (T)Activator.CreateInstance(typeof(T)); ;

            add_entity(toadd);
            return toadd;
        }

        /// <summary>
        /// Add an entity to the internal dictionnaries and increment current_id
        /// </summary>
        /// <param name="entity">Entity to add</param>
        private void add_entity(CorePackage.Global.Definition entity)
        {
            definitions[current_uid] = entity;
            ids[entity] = current_uid++;
        }

        /// <summary>
        /// Remove an entity from the internal dictionaries
        /// </summary>
        /// <remarks>
        /// Throws an InvalidOperationException if trying to remove default added entity
        /// Throws a KeyNotFoundException if an entity is not identified by the given id
        /// </remarks>
        /// <param name="definition_uid">Identifier of the entity to remove</param>
        public void remove_entity(UInt32 definition_uid)
        {
            if (definition_uid <= 5)
                throw new InvalidOperationException("EntityFactory.remove : cannot remove base entities");

            if (!definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.remove : given definition uid hasn't been found");

            ids.Remove(definitions[definition_uid]);
            definitions.Remove(definition_uid);
        }

        /// <summary>
        /// Removes an entity from the internal dictionaries
        /// </summary>
        /// <remarks>
        /// Throws a KeyNotFoundException if the entity hasn't been found
        /// Throws an InvalidOperationException if the given entity is a default added entity
        /// </remarks>
        /// <param name="entity">Entity to remove</param>
        public void remove_entity(CorePackage.Global.Definition entity)
        {
            if (!ids.ContainsKey(entity))
                throw new KeyNotFoundException("EntityFactory.remove : given definition uid hasn't been found");

            uint entity_id = ids[entity];

            if (entity_id <= 5)
                throw new InvalidOperationException("EntityFactory.remove : cannot remove base entities");

            definitions.Remove(entity_id);
            ids.Remove(entity);
        }

        /// <summary>
        /// Find an entity in the dictionaries
        /// </summary>
        /// <remarks>Throws a KeyNotFoundException if entity hasn't been found</remarks>
        /// <param name="definition_uid">Identifier of an entity</param>
        /// <returns>The entity to find</returns>
        public CorePackage.Global.Definition find(UInt32 definition_uid)
        {
            if (!definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.find : given definition uid hasn't been found");

            return definitions[definition_uid];
        }

        /// <summary>
        /// Find basic an entity from its id
        /// </summary>
        /// <param name="definition_uid">Identifier of the basic entity</param>
        /// <returns>Basic entity to find</returns>
        public CorePackage.Global.Definition find(BASE_ID definition_uid)
        {
            return find((uint)definition_uid);
        }

        /// <summary>
        /// Find a definition of a specific type
        /// </summary>
        /// <remarks>Throws an InvalidCastException if the type doesn't match</remarks>
        /// <typeparam name="T">Type of the entity to find</typeparam>
        /// <param name="id">Identifier of the entity to find</param>
        /// <returns>The entity to find</returns>
        public T findDefinitionOfType<T>(UInt32 id) where T : class
        {
            CorePackage.Global.Definition to_find = find(id);
            T to_ret = to_find as T;

            if (to_ret == null)
                throw new InvalidCastException("Unable to cast entity with id " + id.ToString() + " (of type " + to_find.GetType().ToString() + ") into " + typeof(T).ToString());
            return to_ret;
        }

        /// <summary>
        /// Find a declarator of a specific entity type
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarators</typeparam>
        /// <param name="id">Identifier of the declarator to find</param>
        /// <returns>The declarator to find</returns>
        public CorePackage.Global.IDeclarator<T> getDeclaratorOf<T>(UInt32 id)
        {
            return findDefinitionOfType<CorePackage.Global.IDeclarator<T>>(id);
        }

        /// <summary>
        /// Declare an entity into a specific declarator
        /// </summary>
        /// <typeparam name="Entity">Type of the entity to declare</typeparam>
        /// <typeparam name="Declarator">Type used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the declarator in which declare entity</param>
        /// <param name="name">Name of the entity to declare</param>
        /// <param name="visibility">Visibility of the entity to declare</param>
        /// <returns>The identifier of the freshly declared entity</returns>
        public UInt32 declare<Entity, Declarator>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility)
            where Declarator : CorePackage.Global.Definition
            where Entity : Declarator
        {
            getDeclaratorOf<Declarator>(containerID).Declare(create<Entity>(), name, visibility);
            return LastID;
        }

        /// <summary>
        /// Declare an entity into a specfic declarator
        /// </summary>
        /// <remarks>Entity type given must be the same used in the declarator</remarks>
        /// <typeparam name="Entity">Type of the entity to declare</typeparam>
        /// <param name="containerID">Identifier of the container in which declare the entity</param>
        /// <param name="name">Name of the entity to declare</param>
        /// <param name="visibility">Visibility of the entity to declare</param>
        /// <returns>Identifier of the freshly declared entity</returns>
        public UInt32 declare<Entity>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility) where Entity : CorePackage.Global.Definition
        {
            getDeclaratorOf<Entity>(containerID).Declare(create<Entity>(), name, visibility);
            return LastID;
        }

        /// <summary>
        /// Remove an entity from a specific container
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the container from which remove the entity</param>
        /// <param name="name">Name of the entity to remove</param>
        public void remove<T>(UInt32 containerID, string name) where T : CorePackage.Global.Definition
        {
            T entity = getDeclaratorOf<T>(containerID).Pop(name);

            //need to remove internal declared entities from indexes
            //checks :
            //      - IContext => handle contexts and classes
            //      - Function

            remove_entity(entity);

            //could be nice to return removed ids
        }

        /// <summary>
        /// Rename an entity in a specific declarator
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the container in which rename the entity</param>
        /// <param name="lastName">Current name of the entity to rename</param>
        /// <param name="newName">Name to set to the entity</param>
        public void rename<T>(UInt32 containerID, string lastName, string newName) where T : CorePackage.Global.Definition
        {
            getDeclaratorOf<T>(containerID).Rename(lastName, newName);
        }

        /// <summary>
        /// Move an entity from a declarator to another
        /// </summary>
        /// <typeparam name="T">Type of the entity used in declarators</typeparam>
        /// <param name="fromID">Identifier of the declarator that contains the entity</param>
        /// <param name="toID">Identifier of the declarator to which move the entity</param>
        /// <param name="name">Name of the entity to move</param>
        public void move<T>(UInt32 fromID, UInt32 toID, string name)
        {
            CorePackage.Global.IDeclarator<T> from = getDeclaratorOf<T>(fromID);
            CorePackage.Global.IDeclarator<T> to = getDeclaratorOf<T>(toID);

            CorePackage.Global.AccessMode visibility = new CorePackage.Global.AccessMode();
            T definition = from.GetVisibilityOf(name, ref visibility);
            to.Declare(definition, name, visibility);
            from.Pop(name);
        }

        /// <summary>
        /// Change an entity visibility in a specific declarator
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the declarator in which change entity visibility</param>
        /// <param name="name">Name of the entity to which change visibility</param>
        /// <param name="newVisi">New visibility to set</param>
        public void changeVisibility<T>(UInt32 containerID, string name, CorePackage.Global.AccessMode newVisi)
        {
            getDeclaratorOf<T>(containerID).ChangeVisibility(name, newVisi);
        }
    }
}
