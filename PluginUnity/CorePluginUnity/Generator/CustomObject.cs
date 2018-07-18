using Core.Plugin.Unity.Extensions;
using CoreControl;
using System;
using System.Collections.Generic;
using static Core.Plugin.Unity.Generator.GeneratedClassTemplate;
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

        private readonly GeneratedClassTemplate _template = new GeneratedClassTemplate();

        public CustomObject(Entity entity, Controller controller)
        {
            ObjectName = entity.Name;
            _template.ClassName = ObjectName;
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
            // Generate attributes
            GenerateAttributes(entity, controller);
            // Generate methods
            GenerateMethods(entity, controller);
        }

        private void GenerateAttributes(Entity entity, Controller controller)
        {
            foreach (var attrib in Fields)
            {
                _template.Attributes.Add(new GeneratedClassTemplate.Attribute { Name = attrib.Key, Type = GetFieldType(attrib.Value) });
            }
        }

        private void GenerateMethods(Entity entity, Controller controller)
        {
            var methods = controller.GetEntitiesOfType(ENTITY.FUNCTION, entity.Id);
            //template.Outputs.Clear();
            foreach (var item in methods)
            {
                // method is for DNAI call
                var method = new Method
                {
                    Name = item.Name,
                    FunctionId = item.Id
                };

                //template.Functions.Add(func);

                //template.FunctionId = item.Id;
                var pars = controller.GetFunctionParameters(item.Id);

                // Gets the variables with the function container id
                //foreach (var v in pars)
                //{
                //    var typeId = controller.GetVariableType(v.Id);
                //    var realType = controller.GetEntityType(typeId);
                //    if (realType == CoreControl.EntityFactory.ENTITY.ENUM_TYPE)
                //    {
                //        //var ret = $"{enumNames[typeId]} {v.Name}";
                //        //template.Inputs.Add(ret);
                //    }
                //    else
                //    {
                //        var arg = controller.GetEntity(typeId);
                //        var @in = $"{arg.Name} {v.Name}";
                //        var ret = v.ToSerialString(controller);
                //        //template.Inputs.Add(v.ToSerialString(controller));
                //    }
                //}

                for (int i = 0; i < pars.Count; i++)
                {
                    var fType = GetFieldType(controller.GetVariableType(pars[i].Id));
                    //func.FunctionArguments += $"{{\"{pars[i].Name}\", ({controller.GetVariableValue(pars[i].Id).GetType().ToString()}) {pars[i].Name}}},";
                    //method.InvokeArguments += $"{{\"{pars[i].Name}\", ({controller.GetEntity(controller.GetVariableType(pars[i].Id)).Name}) @{pars[i].Name}}},";
                    method.InvokeArguments += $"{{\"{pars[i].Name}\", ({fType}) @{pars[i].Name}}},";
                    //method.Arguments += controller.GetEntity(controller.GetVariableType(pars[i].Id)).Name + " @" + pars[i].Name + (i + 1 >= pars.Count ? "" : ",");
                    method.Arguments += fType + " @" + pars[i].Name + (i + 1 >= pars.Count ? "" : ",");
                }

                // TODO : handle tuple return
                foreach (var ret in controller.GetFunctionReturns(item.Id))
                {
                    var r = controller.GetEntity(controller.GetVariableType(ret.Id)).Name;
                    var fType = GetFieldType(controller.GetVariableType(ret.Id));
                    method.Return = fType;
                    method.InvokeReturn = ret.Name;
                    //template.Outputs.Add(ret.ToSerialString(controller));
                }
                _template.Methods.Add(method);
            }
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

        /// <summary>
        /// Returns the class in a string format.
        /// </summary>
        /// <returns></returns>
        public string GetGeneratedClass()
        {
            return _template.TransformText();
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

    /// <summary>
    /// Implements the partial class generated by the .tt template.
    /// </summary>
    public partial class GeneratedClassTemplate
    {
        public string ClassName = "GeneratedClass";
        public List<Method> Methods = new List<Method>();
        public List<Attribute> Attributes = new List<Attribute>();

        public class Method
        {
            public string Return = "void";
            public string InvokeReturn = "result";
            public string Name = "GeneratedMethod";
            public string Arguments = "";
            public string InvokeArguments = "";
            public uint FunctionId;
        }

        public class Attribute
        {
            public string Name;
            public string Type;
        }
    }
}