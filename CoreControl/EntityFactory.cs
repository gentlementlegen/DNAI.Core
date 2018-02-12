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
        /// Enumeration that represents an entity visibility at declaration
        /// </summary>
        /// <remarks>Makes a transition in order to hide CorePackage.Global.AccessMode enum</remarks>
        public enum VISIBILITY
        {
            PUBLIC = CorePackage.Global.AccessMode.EXTERNAL,
            PRIVATE = CorePackage.Global.AccessMode.INTERNAL
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

        public struct Entity
        {
            public EntityFactory.ENTITY Type { get; set; }

            public string Name { get; set; }

            public UInt32 Id { get; set; }

        }

        /// <summary>
        /// Associates an id to its entity definition
        /// </summary>
        private readonly Dictionary<UInt32, CorePackage.Global.Definition> definitions = new Dictionary<uint, CorePackage.Global.Definition>();

        /// <summary>
        /// Associates an entity definition to its id
        /// </summary>
        private readonly Dictionary<CorePackage.Global.Definition, UInt32> ids = new Dictionary<CorePackage.Global.Definition, uint>();

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

            //boolean type is in 1
            AddEntity(CorePackage.Entity.Type.Scalar.Boolean);

            //integer type is in 2
            AddEntity(CorePackage.Entity.Type.Scalar.Integer);

            //floating type is in 3
            AddEntity(CorePackage.Entity.Type.Scalar.Floating);

            //character type is in 4
            AddEntity(CorePackage.Entity.Type.Scalar.Character);

            //string type is in 5
            AddEntity(CorePackage.Entity.Type.Scalar.String);
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
        public T Create<T>() where T : CorePackage.Global.Definition
        {
            T toadd = (T)Activator.CreateInstance(typeof(T));

            AddEntity(toadd);
            return toadd;
        }

        /// <summary>
        /// Add an entity to the internal dictionnaries and increment current_id
        /// </summary>
        /// <param name="entity">Entity to add</param>
        private void AddEntity(CorePackage.Global.Definition entity)
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
        public void RemoveEntity(UInt32 definition_uid)
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
        public void RemoveEntity(CorePackage.Global.Definition entity)
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
        public CorePackage.Global.Definition Find(UInt32 definition_uid)
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
        public CorePackage.Global.Definition Find(BASE_ID definition_uid)
        {
            return Find((uint)definition_uid);
        }

        /// <summary>
        /// Returns the id of an entity in the internal dicitonaries
        /// </summary>
        /// <param name="entity">Entity for which find the id</param>
        /// <returns>Id of the given entity</returns>
        public UInt32 GetEntityID(CorePackage.Global.Definition entity)
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
            CorePackage.Global.Definition to_find = Find(id);
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
        public CorePackage.Global.IDeclarator<T> GetDeclaratorOf<T>(UInt32 id)
        {
            return FindDefinitionOfType<CorePackage.Global.IDeclarator<T>>(id);
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
        public UInt32 Declare<Entity, Declarator>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility)
            where Declarator : CorePackage.Global.Definition
            where Entity : Declarator
        {
            GetDeclaratorOf<Declarator>(containerID).Declare(Create<Entity>(), name, visibility);
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
        public UInt32 Declare<Entity>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility) where Entity : CorePackage.Global.Definition
        {
            GetDeclaratorOf<Entity>(containerID).Declare(Create<Entity>(), name, visibility);
            return LastID;
        }

        /// <summary>
        /// Remove an entity from a specific container
        /// </summary>
        /// <typeparam name="T">Type of the entity used in the declarator</typeparam>
        /// <param name="containerID">Identifier of the container from which remove the entity</param>
        /// <param name="name">Name of the entity to remove</param>
        /// <returns>List of all removed entities' id</returns>
        public List<UInt32> Remove<T>(UInt32 containerID, string name) where T : CorePackage.Global.Definition
        {
            T entity = GetDeclaratorOf<T>(containerID).Pop(name);
            List<UInt32> removed = new List<uint> { GetEntityID(entity) };

            RemoveEntity(entity);

            CorePackage.Global.IContext ctx = entity as CorePackage.Global.IContext;

            if (ctx != null)
            {
                List<CorePackage.Global.IContext> ctxs = ((CorePackage.Global.IDeclarator<CorePackage.Global.IContext>)ctx).Clear();
                List<CorePackage.Entity.Function> fnts = ((CorePackage.Global.IDeclarator<CorePackage.Entity.Function>)ctx).Clear();
                List<CorePackage.Entity.Variable> vars = ((CorePackage.Global.IDeclarator<CorePackage.Entity.Variable>)ctx).Clear();
                List<CorePackage.Entity.DataType> types = ((CorePackage.Global.IDeclarator<CorePackage.Entity.DataType>)ctx).Clear();

                foreach (CorePackage.Global.IContext curr in ctxs) { removed.Add(GetEntityID(curr)); }
                foreach (CorePackage.Entity.Function curr in fnts) { removed.Add(GetEntityID(curr)); }
                foreach (CorePackage.Entity.Variable curr in vars) { removed.Add(GetEntityID(curr)); }
                foreach (CorePackage.Entity.DataType curr in types) { removed.Add(GetEntityID(curr)); }
            }
            else
            {
                CorePackage.Entity.Function fnt = entity as CorePackage.Entity.Function;

                if (fnt != null)
                {
                    List<CorePackage.Entity.Variable> vars = fnt.Clear();

                    foreach (CorePackage.Entity.Variable curr in vars) { removed.Add(GetEntityID(curr)); }
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
        public void Rename<T>(UInt32 containerID, string lastName, string newName) where T : CorePackage.Global.Definition
        {
            GetDeclaratorOf<T>(containerID).Rename(lastName, newName);
        }

        /// <summary>
        /// Move an entity from a declarator to another
        /// </summary>
        /// <typeparam name="T">Type of the entity used in declarators</typeparam>
        /// <param name="fromID">Identifier of the declarator that contains the entity</param>
        /// <param name="toID">Identifier of the declarator to which move the entity</param>
        /// <param name="name">Name of the entity to move</param>
        public void Move<T>(UInt32 fromID, UInt32 toID, string name)
        {
            CorePackage.Global.IDeclarator<T> from = GetDeclaratorOf<T>(fromID);
            CorePackage.Global.IDeclarator<T> to = GetDeclaratorOf<T>(toID);

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
        public void ChangeVisibility<T>(UInt32 containerID, string name, CorePackage.Global.AccessMode newVisi)
        {
            GetDeclaratorOf<T>(containerID).ChangeVisibility(name, newVisi);
        }
        
        /// <summary>
        /// Get the list of entities of a speficic type in a specific container
        /// </summary>
        /// <typeparam name="T">Type of entities to find in container</typeparam>
        /// <param name="containerID">Identifier of the container</param>
        /// <returns>List of declared entities</returns>
        public Dictionary<string, dynamic> GetEntitiesOfType<T>(UInt32 containerID)
            where T : CorePackage.Global.Definition
        {
            Dictionary<string, T> real = GetDeclaratorOf<T>(containerID).GetEntities(CorePackage.Global.AccessMode.EXTERNAL);
            Dictionary<string, dynamic> to_ret = new Dictionary<string, dynamic>();

            foreach (KeyValuePair<string, T> curr in real)
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
                    return factory.Declare<CorePackage.Entity.Context, CorePackage.Global.IContext>(containerID, name, (CorePackage.Global.AccessMode)visibility);
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
                    return factory.Declare<CorePackage.Entity.Type.EnumType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.OBJECT_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Type.ObjectType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.LIST_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.Declare<CorePackage.Entity.Type.ListType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
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

        /// <summary>
        /// Associates an EntityFactory.erase function to a specific ENTITY key
        /// </summary>
        static private readonly Dictionary<ENTITY, Func<EntityFactory, UInt32, string, List<UInt32>>> erasers = new Dictionary<ENTITY, Func<EntityFactory, uint, string, List<UInt32>>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.Remove<CorePackage.Global.IContext>(containerID, name);
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.Remove<CorePackage.Entity.Variable>(containerID, name);
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.Remove<CorePackage.Entity.Function>(containerID, name);
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.Remove<CorePackage.Entity.DataType>(containerID, name);
                }
            }
        };

        /// <summary>
        /// Remove an entity declared in a container
        /// </summary>
        /// <param name="to_remove">Type of entity contained in the declarator</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="name">Name of the entity to remove in the container</param>
        /// <returns>List of all removed entities' id</returns>
        public List<UInt32> Remove(ENTITY to_remove, UInt32 containerID, string name)
        {
            if (!erasers.ContainsKey(to_remove))
                throw new KeyNotFoundException("No such eraser for ENTITY: " + to_remove.ToString());
            return erasers[to_remove].Invoke(this, containerID, name);
        }

        /// <summary>
        /// Associates an EntityFactory.rename function to a specific ENTITY key
        /// </summary>
        static private readonly Dictionary<ENTITY, Func<EntityFactory, UInt32, string, string, bool>> renamers = new Dictionary<ENTITY, Func<EntityFactory, uint, string, string, bool>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.Rename<CorePackage.Global.IContext>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.Rename<CorePackage.Entity.Variable>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.Rename<CorePackage.Entity.Function>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.Rename<CorePackage.Entity.DataType>(containerID, lastName, newName);
                    return true;
                }
            }
        };

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
                            var dec = GetDeclaratorOf<CorePackage.Global.IContext>(id.Value);
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
            if (!renamers.ContainsKey(to_rename))
                throw new KeyNotFoundException("No such renamer for ENTITY: " + to_rename.ToString());
            renamers[to_rename].Invoke(this, containerID, lastName, newName);
        }

        /// <summary>
        /// Associates an EntityFactory.move function to a specific ENTITY key
        /// </summary>
        static private readonly Dictionary<ENTITY, Func<EntityFactory, UInt32, UInt32, string, bool>> movers = new Dictionary<ENTITY, Func<EntityFactory, uint, uint, string, bool>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.Move<CorePackage.Global.IContext>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.Move<CorePackage.Entity.Variable>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.Move<CorePackage.Entity.Function>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.Move<CorePackage.Entity.DataType>(fromID, toID, name);
                    return true;
                }
            }
        };

        /// <summary>
        /// Move an entity from a specific container to another
        /// </summary>
        /// <param name="to_move">Type of the entity contained in the declarator</param>
        /// <param name="fromID">Identifier of the entity declared in the container</param>
        /// <param name="toID">Identifier of the declarator in which move the entity</param>
        /// <param name="name">Name of the entity to move</param>
        public void Move(ENTITY to_move, UInt32 fromID, UInt32 toID, string name)
        {
            if (!movers.ContainsKey(to_move))
                throw new KeyNotFoundException("No such mover for ENTITY: " + to_move.ToString());
            movers[to_move].Invoke(this, fromID, toID, name);
        }

        /// <summary>
        /// Associates an EntityFactory.changeVisibility function to a specific ENTITY key
        /// </summary>
        private Dictionary<ENTITY, Func<EntityFactory, UInt32, string, VISIBILITY, bool>> visi_modifiers = new Dictionary<ENTITY, Func<EntityFactory, uint, string, VISIBILITY, bool>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.ChangeVisibility<CorePackage.Global.IContext>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.ChangeVisibility<CorePackage.Entity.Variable>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.ChangeVisibility<CorePackage.Entity.Function>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.ChangeVisibility<CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            }
        };

        /// <summary>
        /// Change an entity visibility declared in a specific container
        /// </summary>
        /// <param name="to_change_visi">Type of the entity to change visibility</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="name">Name of the declared entity in the container</param>
        /// <param name="newVisi">New visibility of the declared entity</param>
        public void ChangeVisibility(ENTITY to_change_visi, UInt32 containerID, string name, VISIBILITY newVisi)
        {
            if (!visi_modifiers.ContainsKey(to_change_visi))
                throw new KeyNotFoundException("No such visibility modifier for ENTITY: " + to_change_visi.ToString());
            visi_modifiers[to_change_visi].Invoke(this, containerID, name, newVisi);
        }

        /// <summary>
        /// Dictionnary used to define each type to use on GetEntitiesOfType method
        /// </summary>
        private Dictionary<ENTITY, Func<EntityFactory, UInt32, Dictionary<string, dynamic>>> entities_getters = new Dictionary<ENTITY, Func<EntityFactory, UInt32, Dictionary<string, dynamic>>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 id) =>
                {
                    return factory.GetEntitiesOfType<CorePackage.Global.IContext>(id);
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 id) =>
                {
                    return factory.GetEntitiesOfType<CorePackage.Entity.DataType>(id);
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 id) =>
                {
                    return factory.GetEntitiesOfType<CorePackage.Entity.Function>(id);
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 id) =>
                {
                    return factory.GetEntitiesOfType<CorePackage.Entity.Variable>(id);
                }
            }
        };

        /// <summary>
        /// Method to get entities of a specific type in a given container
        /// </summary>
        /// <param name="entities_type">Type of the entities to return</param>
        /// <param name="containerID">Identifier of the container in which entities are declared</param>
        /// <returns>List of declared entities</returns>
        public List<Entity> GetEntitiesOfType(ENTITY entities_type, UInt32 containerID)
        {
            if (!entities_getters.ContainsKey(entities_type))
                throw new KeyNotFoundException("No such entity getter for ENTITY : " + entities_type.ToString());

            Dictionary<string, dynamic> entities = entities_getters[entities_type].Invoke(this, containerID);
            List<Entity> to_ret = new List<Entity>();

            foreach (KeyValuePair<string, dynamic> curr in entities)
            {
                to_ret.Add(new Entity
                {
                    Id = GetEntityID(curr.Value),
                    Name = curr.Key,
                    Type = entities_type
                });
            }
            return to_ret;
        }
    }
}