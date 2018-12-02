using CoreControl.SerializationModel;
using CorePackage.Entity.Type;
using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreControl
{
    /// <summary>
    /// Class that is used to manipulate entities
    /// </summary>
    public class EntityFactory
    {
        public static UInt32 MagicNumber = 0xFA7BEA57; //FATBEAST

        /// <summary>
        /// Ids of entities created by default
        /// </summary>
        public enum BASE_ID : uint
        {
            GLOBAL_CTX = 0,
            BOOLEAN_TYPE,
            INTEGER_TYPE,
            FLOATING_TYPE,
            CHARACTER_TYPE,
            STRING_TYPE,
            DICT_TYPE,
            ANY_TYPE,
            MATRIX_TYPE,
            RESSOURCE_TYPE
        }

        /// <summary>
        /// Enumeration that represents an entity visibility at declaration
        /// </summary>
        /// <remarks>Makes a transition in order to hide CorePackage.Global.AccessMode enum</remarks>
        public enum VISIBILITY
        {
            PUBLIC = AccessMode.EXTERNAL,
            PRIVATE = AccessMode.INTERNAL
        }

        /// <summary>
        /// Enumeration that represents a declarable entity
        /// </summary>
        public enum ENTITY
        {
            CONTEXT,
            VARIABLE,
            FUNCTION,
            DATA_TYPE,
            ENUM_TYPE,
            OBJECT_TYPE,
            LIST_TYPE
        }

        [Flags]
        public enum EntityType
        {
            ALL = 0,
            PUBLIC = 1,
            PRIVATE = 2,
            CONTEXT = 4,
            FUNCTION = 8
        }

        public class Entity
        {
            public ENTITY Type { get; set; }

            public string Name { get; set; }

            public uint Id { get; set; }

            public VISIBILITY Visibility { get; set; }
        }

        public Dictionary<uint, IDefinition> Definitions { get; } = new Dictionary<uint, IDefinition>();

        /// <summary>
        /// Associates an entity definition to its id
        /// </summary>
        private Dictionary<IDefinition, uint> ids = new Dictionary<IDefinition, uint>();

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
            AddEntity(new CorePackage.Entity.Context());
            IDeclarator root = GetDeclarator(0);
            (root as IDefinition).Name = "";

            //boolean type is in 1
            AddEntity(Builtins.Boolean);
            Builtins.Boolean.Parent = root;
            Builtins.Boolean.Name = "Bool";
            root.Declare(Builtins.Boolean, "Bool", AccessMode.EXTERNAL);

            //integer type is in 2
            AddEntity(Builtins.Integer);
            Builtins.Integer.Parent = root;
            Builtins.Integer.Name = "Integer";
            root.Declare(Builtins.Integer, "Integer", AccessMode.EXTERNAL);

            //floating type is in 3
            AddEntity(Builtins.Floating);
            Builtins.Floating.Parent = root;
            Builtins.Floating.Name = "Floating";
            root.Declare(Builtins.Floating, "Floating", AccessMode.EXTERNAL);

            //character type is in 4
            AddEntity(Builtins.Character);
            Builtins.Character.Parent = root;
            Builtins.Character.Name = "Character";
            root.Declare(Builtins.Character, "Character", AccessMode.EXTERNAL);

            //string type is in 5
            AddEntity(Builtins.String);
            Builtins.String.Parent = root;
            Builtins.String.Name = "String";
            root.Declare(Builtins.String, "String", AccessMode.EXTERNAL);

            //dict type is in 6
            AddEntity(Builtins.Dictionnary);
            Builtins.Dictionnary.Parent = root;
            Builtins.Dictionnary.Name = "Dict";
            root.Declare(Builtins.Dictionnary, "Dict", AccessMode.EXTERNAL);

            //any type is in 7
            AddEntity(Builtins.Any);
            Builtins.Any.Parent = root;
            Builtins.Any.Name = "Any";
            root.Declare(Builtins.Any, "Any", AccessMode.EXTERNAL);

            //matrix type is in 8
            AddEntity(Builtins.Matrix);
            Builtins.Matrix.Parent = root;
            Builtins.Matrix.Name = "Matrix";
            root.Declare(Builtins.Matrix, "Matrix", AccessMode.EXTERNAL);

            //ressource type is in 9
            AddEntity(Builtins.Resource);
            Builtins.Resource.Parent = root;
            Builtins.Resource.Name = "Ressource";
            root.Declare(Builtins.Resource, "Ressource", AccessMode.EXTERNAL);
        }

        /// <summary>
        /// Getter for the current id
        /// </summary>
        public UInt32 Size
        {
            get { return current_uid; }
        }

        /// <summary>
        /// Getter for the id of the last entity declared
        /// </summary>
        public UInt32 LastID
        {
            get { return Size - 1; }
        }

        /// <summary>
        /// Creates an entity and add it the the dictionnaries
        /// </summary>
        /// <typeparam name="T">Type of the entity to declare</typeparam>
        /// <returns>Freshly instanciated entity</returns>
        public T Create<T>() where T : IDefinition
        {
            T toadd = (T)Activator.CreateInstance(typeof(T));

            AddEntity(toadd);
            return toadd;
        }

        /// <summary>
        /// Add an entity to the internal dictionnaries and increment current_id
        /// </summary>
        /// <param name="entity">Entity to add</param>
        public void AddEntity(CorePackage.Global.IDefinition entity)
        {
            Definitions[current_uid] = entity;
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
        public void RemoveEntity(UInt32 definition_uid)
        {
            if (definition_uid <= 5)
                throw new InvalidOperationException("EntityFactory.remove : cannot remove base entities");

            if (!Definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.remove : given definition uid hasn't been found");

            ids.Remove(Definitions[definition_uid]);
            Definitions.Remove(definition_uid);
        }

        /// <summary>
        /// Removes an entity from the internal dictionaries
        /// </summary>
        /// <remarks>
        /// Throws a KeyNotFoundException if the entity hasn't been found
        /// Throws an InvalidOperationException if the given entity is a default added entity
        /// </remarks>
        /// <param name="entity">Entity to remove</param>
        public void RemoveEntity(CorePackage.Global.IDefinition entity)
        {
            if (!ids.ContainsKey(entity))
                throw new KeyNotFoundException("EntityFactory.remove : given definition uid hasn't been found");

            uint entity_id = ids[entity];

            if (entity_id <= 5)
                throw new InvalidOperationException("EntityFactory.remove : cannot remove base entities");

            Definitions.Remove(entity_id);
            ids.Remove(entity);
        }

        /// <summary>
        /// Find an entity in the dictionaries
        /// </summary>
        /// <remarks>Throws a KeyNotFoundException if entity hasn't been found</remarks>
        /// <param name="definition_uid">Identifier of an entity</param>
        /// <returns>The entity to find</returns>
        public IDefinition Find(UInt32 definition_uid)
        {
            if (!Definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.find : given definition of id " + definition_uid.ToString() + " hasn't been found");

            return Definitions[definition_uid];
        }

        /// <summary>
        /// Find basic an entity from its id
        /// </summary>
        /// <param name="definition_uid">Identifier of the basic entity</param>
        /// <returns>Basic entity to find</returns>
        public IDefinition Find(BASE_ID definition_uid)
        {
            return Find((uint)definition_uid);
        }

        /// <summary>
        /// Returns the id of an entity in the internal dicitonaries
        /// </summary>
        /// <param name="entity">Entity for which find the id</param>
        /// <returns>Id of the given entity</returns>
        public UInt32 GetEntityID(CorePackage.Global.IDefinition entity)
        {
            if (ids.ContainsKey(entity))
                return ids[entity];
            throw new KeyNotFoundException("EntityFactory.getEntityID : No such id for the given entity");
        }

        /// <summary>
        /// Find a definition of a specific type
        /// </summary>
        /// <remarks>Throws an InvalidCastException if the type doesn't match</remarks>
        /// <typeparam name="T">Type of the entity to find</typeparam>
        /// <param name="id">Identifier of the entity to find</param>
        /// <returns>The entity to find</returns>
        public T FindDefinitionOfType<T>(UInt32 id) where T : class
        {
            IDefinition to_find = Find(id);
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
        public CorePackage.Global.IDeclarator GetDeclarator(UInt32 id)
        {
            return FindDefinitionOfType<CorePackage.Global.IDeclarator>(id);
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
        public UInt32 Declare<Entity>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility)
            where Entity : IDefinition
        {
            Entity todecl = Create<Entity>();

            todecl.Name = name;
            todecl.Parent = GetDeclarator(containerID);
            todecl.Parent.Declare(todecl, name, visibility);
            return LastID;
        }

        /// <summary>
        /// Remove an entity from a specific container
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the container from which remove the entity</param>
        /// <param name="name">Name of the entity to remove</param>
        /// <returns>List of all removed entities' id</returns>
        public List<UInt32> Remove(UInt32 containerID, string name)
        {
            CorePackage.Global.IDefinition entity = GetDeclarator(containerID).Pop(name);
            List<UInt32> removed = new List<uint> { GetEntityID(entity) };

            RemoveEntity(entity);

            Stack<CorePackage.Global.IDeclarator> toclear = new Stack<CorePackage.Global.IDeclarator>();

            CorePackage.Global.IDeclarator decl = entity as CorePackage.Global.IDeclarator;

            if (decl != null)
                toclear.Push(decl);

            while (toclear.Count > 0)
            {
                CorePackage.Global.IDeclarator declarator = toclear.Pop();

                foreach (CorePackage.Global.IDefinition curr in declarator.Clear())
                {
                    UInt32 id = GetEntityID(curr);
                    removed.Add(id);
                    RemoveEntity(id);
                    decl = curr as CorePackage.Global.IDeclarator;
                    if (decl != null)
                        toclear.Push(decl);
                }
            }
            return removed;
        }

        /// <summary>
        /// Rename an entity in a specific declarator
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the container in which rename the entity</param>
        /// <param name="lastName">Current name of the entity to rename</param>
        /// <param name="newName">Name to set to the entity</param>
        public void Rename(UInt32 containerID, string lastName, string newName)
        {
            CorePackage.Global.IDeclarator decl = GetDeclarator(containerID);
            CorePackage.Global.IDefinition def = decl.Find(lastName);

            decl.Rename(lastName, newName);
            def.Name = newName; //rename here because declarator do not edit Definition name
        }

        /// <summary>
        /// Move an entity from a declarator to another
        /// </summary>
        /// <typeparam name="T">Type of the entity used in declarators</typeparam>
        /// <param name="fromID">Identifier of the declarator that contains the entity</param>
        /// <param name="toID">Identifier of the declarator to which move the entity</param>
        /// <param name="name">Name of the entity to move</param>
        public void Move(UInt32 fromID, UInt32 toID, string name)
        {
            CorePackage.Global.IDeclarator from = GetDeclarator(fromID);
            CorePackage.Global.IDeclarator to = GetDeclarator(toID);

            CorePackage.Global.AccessMode visibility = from.GetVisibilityOf(name);
            CorePackage.Global.IDefinition definition = from.Pop(name);
            to.Declare(definition, name, visibility);
        }

        /// <summary>
        /// Change an entity visibility in a specific declarator
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the declarator in which change entity visibility</param>
        /// <param name="name">Name of the entity to which change visibility</param>
        /// <param name="newVisi">New visibility to set</param>
        public void ChangeVisibility(UInt32 containerID, string name, VISIBILITY newVisi)
        {
            GetDeclarator(containerID).ChangeVisibility(name, (CorePackage.Global.AccessMode)newVisi);
        }
        
        /// <summary>
        /// Get the list of entities of a speficic type in a specific container
        /// </summary>
        /// <typeparam name="T">Type of entities to find in container</typeparam>
        /// <param name="containerID">Identifier of the container</param>
        /// <returns>List of declared entities</returns>
        public Dictionary<string, dynamic> GetEntitiesOfType(UInt32 containerID)
        {
            Dictionary<string, CorePackage.Global.IDefinition> real = GetDeclarator(containerID).GetEntities(CorePackage.Global.AccessMode.EXTERNAL);
            Dictionary<string, dynamic> to_ret = new Dictionary<string, dynamic>();

            foreach (KeyValuePair<string, CorePackage.Global.IDefinition> curr in real)
            {
                to_ret.Add(curr.Key, curr.Value);
            }
            return to_ret;
        }

        /// <summary>
        /// Associates a EntityFactory.declare function to a specific ENTITY key
        /// </summary>
        static private readonly Dictionary<ENTITY, Func<EntityFactory, UInt32, string, VISIBILITY, UInt32>> declarators = new Dictionary<ENTITY, Func<EntityFactory, uint, string, VISIBILITY, uint>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Context>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Variable>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Function>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.ENUM_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Type.EnumType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.OBJECT_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Type.ObjectType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.LIST_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Type.ListType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            }
        };

        /// <summary>
        /// Will declare an entity in a container with a specific name and visibility
        /// </summary>
        /// <param name="to_declare">Type of the entity to declare</param>
        /// <param name="containerID">Identifier of the container in which declare the entity</param>
        /// <param name="name">Name of the declared entity</param>
        /// <param name="visibility">Visibility of the declared entity</param>
        /// <returns>Identifier of the freshly declared entity</returns>
        public UInt32 Declare(ENTITY to_declare, UInt32 containerID, string name, VISIBILITY visibility)
        {
            if (declarators.ContainsKey(to_declare))
                return declarators[to_declare].Invoke(this, containerID, name, visibility);
            throw new KeyNotFoundException("No such declarator for ENTITY: " + to_declare.ToString());
        }

        private static readonly Dictionary<ENTITY, System.Type> entity_types = new Dictionary<ENTITY, Type>
        {
            { ENTITY.CONTEXT, typeof(CorePackage.Entity.Context) },
            { ENTITY.DATA_TYPE, typeof(CorePackage.Entity.DataType) },
            { ENTITY.FUNCTION, typeof(CorePackage.Entity.Function) },
            { ENTITY.VARIABLE, typeof(CorePackage.Entity.Variable) },
            { ENTITY.OBJECT_TYPE, typeof(CorePackage.Entity.Type.ObjectType) },
            { ENTITY.ENUM_TYPE, typeof(CorePackage.Entity.Type.EnumType) },
            { ENTITY.LIST_TYPE, typeof(CorePackage.Entity.Type.ListType) }
        };

        /// <summary>
        /// Method to get entities of a specific type in a given container
        /// </summary>
        /// <param name="entities_type">Type of the entities to return</param>
        /// <param name="containerID">Identifier of the container in which entities are declared</param>
        /// <returns>List of declared entities</returns>
        public List<Entity> GetEntitiesOfType(ENTITY entities_type, UInt32 containerID)
        {
            Dictionary<string, dynamic> entities = GetEntitiesOfType(containerID);
            List<Entity> to_ret = new List<Entity>();
            System.Type tocheck = entity_types[entities_type];

            foreach (KeyValuePair<string, dynamic> curr in entities)
            {
                if (tocheck.IsAssignableFrom(curr.Value.GetType()))
                    to_ret.Add(new Entity
                    {
                        Id = GetEntityID(curr.Value),
                        Name = curr.Key,
                        Type = entities_type
                    });
            }
            return to_ret;
        }

        /// <summary>
        /// Retrieves entity ids according to the given filters.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        internal List<uint> GetIds(EntityType flags)
        {
            var ret = new List<uint>();

            foreach (var id in ids)
            {
                if ((flags & EntityType.CONTEXT) != 0)
                {
                    try
                    {
                        if (id.Key.GetType() == typeof(CorePackage.Entity.Context))
                        {
                            var dec = GetDeclarator(id.Value);
                            // TODO : check acessibility
                            //if ((flags & EntityType.PUBLIC) != 0)
                                ret.Add(id.Value);
                        }
                    }
                    catch (CorePackage.Error.NotFoundException)
                    {
                    }
                }
                if ((flags & EntityType.FUNCTION) != 0 && id.Key.GetType() == typeof(CorePackage.Entity.Function))
                {
                    ret.Add(id.Value);
                }
            }
            return ret;
        }

        /// <summary>
        /// Rename an entity declared in a container
        /// </summary>
        /// <param name="to_rename">Type of the entity contained in the declarator</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="lastName">Current name of the entity</param>
        /// <param name="newName">New name to set</param>
        public void Rename(ENTITY to_rename, UInt32 containerID, string lastName, string newName)
        {
            Rename(containerID, lastName, newName);
        }

        /// <summary>
        /// Find the type of the entity given
        /// </summary>
        /// <param name="entityId">Id of the entity</param>
        /// <returns>Type of the entity</returns>
        public ENTITY GetEntityType(UInt32 entityId)
        {
            Definition entity = FindDefinitionOfType<Definition>(entityId);

            if (entity.GetType() == typeof(CorePackage.Entity.Type.EnumType))
                return ENTITY.ENUM_TYPE;
            else if (entity.GetType() == typeof(CorePackage.Entity.Type.ObjectType))
                return ENTITY.OBJECT_TYPE;
            else if (entity.GetType() == typeof(CorePackage.Entity.Type.ListType))
                return ENTITY.LIST_TYPE;
            else if (typeof(CorePackage.Entity.DataType).IsAssignableFrom(entity.GetType()))
                return ENTITY.DATA_TYPE;
            else if (entity.GetType() == typeof(CorePackage.Entity.Function))
                return ENTITY.FUNCTION;
            else if (entity.GetType() == typeof(CorePackage.Entity.Variable))
                return ENTITY.VARIABLE;
            else if (entity.GetType() == typeof(CorePackage.Entity.Context))
                return ENTITY.CONTEXT;
            throw new InvalidOperationException("Controller.GetEntityType : Entity " + entity.FullName + " as invalid entity type " + entity.GetType().ToString());
        }

        public void merge(EntityFactory factory)
        {
            var values = typeof(BASE_ID).GetEnumValues();
            uint maxBaseId = (uint)values.GetValue(values.Length - 1);

            foreach (KeyValuePair<uint, IDefinition> curr in factory.Definitions)
            {
                if (curr.Key > maxBaseId)
                {
                    AddEntity(curr.Value);
                }
            }

            CorePackage.Entity.Context globalContext = (CorePackage.Entity.Context)Definitions[0];
            CorePackage.Entity.Context factoryContext = (CorePackage.Entity.Context)factory.Definitions[0];

            foreach (KeyValuePair<string, IDefinition> curr in factoryContext.GetEntities())
            {
                string key = curr.Key;
                while (globalContext.Contains(key)) key += "_copy"; //handles circular references

                globalContext.Declare(curr.Value, key, factoryContext.GetVisibilityOf(curr.Key));
            }
        }

    }
}