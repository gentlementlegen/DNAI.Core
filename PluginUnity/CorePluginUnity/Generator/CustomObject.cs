using CoreControl;
using System;
using System.Collections.Generic;
using static CoreControl.EntityFactory;

namespace Core.Plugin.Unity.Generator
{
    /// <summary>
    /// Wrapper for custom DNAI objects.
    /// </summary>
    public class CustomObject : IEquatable<CustomObject>
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// The type of the object.
        /// </summary>
        public string ObjectType { get; private set; }

        /// <summary>
        /// The object id in the associated controller.
        /// </summary>
        public uint ObjectId { get; }

        /// <summary>
        /// The original entity.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// The list of fields of the entity.
        /// Key represents the field name, Value represents the field id.
        /// </summary>
        public IReadOnlyDictionary<string, uint> Fields { get; }

        /// <summary>
        /// List of custom registered classes.
        /// </summary>
        private static readonly Dictionary<uint, string> _registeredClasses = new Dictionary<uint, string>();

        public CustomObject(Entity entity, Controller controller)
        {
            ObjectName = entity.Name;
            ObjectId = entity.Id;
            _registeredClasses.Add(ObjectId, ObjectName);
            Entity = entity;
            ObjectType = entity.Name;
            var fields = new Dictionary<string, uint>();
            var attribs = controller.InstantiateType(ObjectId);
            foreach (var attrib in controller.GetClassAttributes(ObjectId))
            {
                //var objType = SetObjectType((BASE_ID)controller.GetClassAttribute(ObjectId, attrib));
                //if (!string.IsNullOrEmpty(objType))
                //{
                //    fields.Add(attrib, objType);
                //}
                fields.Add(attrib, controller.GetClassAttribute(ObjectId, attrib));
            }
            Fields = fields;
        }

        /// <summary>
        /// Get the C# corresponding type from a given field index.
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public string GetFieldType(uint fieldId)
        {
            return GetObjectType((BASE_ID)fieldId);
        }

        private string GetObjectType(EntityFactory.BASE_ID typeId)
        {
            switch (typeId)
            {
                case EntityFactory.BASE_ID.BOOLEAN_TYPE:
                    return ObjectType = "bool";

                case EntityFactory.BASE_ID.INTEGER_TYPE:
                    return ObjectType = "int";

                case EntityFactory.BASE_ID.FLOATING_TYPE:
                    return ObjectType = "float";

                case EntityFactory.BASE_ID.CHARACTER_TYPE:
                    return ObjectType = "char";

                case EntityFactory.BASE_ID.STRING_TYPE:
                    return ObjectType = "string";

                default:
                    if (_registeredClasses.ContainsKey((uint)typeId))
                        return _registeredClasses[(uint)typeId];
                    return "object";
            }
        }

        public bool Equals(CustomObject other)
        {
            throw new NotImplementedException();
        }

        public static bool operator ==(CustomObject obj, CustomObject other)
        {
            return false;
        }

        public static bool operator !=(CustomObject obj, CustomObject other)
        {
            return false;
        }
    }
}