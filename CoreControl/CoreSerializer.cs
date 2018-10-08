using CorePackage.Entity.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl
{
    public class CoreSerializer
    {
        public EntityFactory Factory { get; set; }

        public void SaveTo(string filename)
        {
            var file = new CoreFile
            {
                MagicNumber = EntityFactory.MagicNumber,
                Entities = new List<SerializationModel.Entity>()
            };
            List<dynamic> reals = new List<dynamic>();

            foreach (var defPair in Factory.Definitions)
            {
                var toadd = new SerializationModel.Entity
                {
                    Id = defPair.Key,
                    Name = defPair.Value.Name,
                    Type = Factory.GetEntityType(defPair.Key),
                    Visibility = (EntityFactory.VISIBILITY)defPair.Value.Parent.GetVisibilityOf(defPair.Value.Name)
                };
                CorePackage.Global.IDefinition definition = defPair.Value;
                dynamic real;

                file.Entities.Add(toadd);
                switch (toadd.Type)
                {
                    case EntityFactory.ENTITY.CONTEXT:
                        real = GetContextFrom((CorePackage.Entity.Context)definition);
                        break;
                    case EntityFactory.ENTITY.DATA_TYPE:
                        real = GetDataTypeFrom((CorePackage.Entity.DataType)definition);
                        break;
                    case EntityFactory.ENTITY.ENUM_TYPE:
                        real = GetEnumFrom((EnumType)definition);
                        break;
                    case EntityFactory.ENTITY.FUNCTION:
                        break;
                    case EntityFactory.ENTITY.LIST_TYPE:
                        break;
                    case EntityFactory.ENTITY.OBJECT_TYPE:
                        break;
                    case EntityFactory.ENTITY.VARIABLE:
                        break;
                }
            }
        }

        #region SaveHelpers

        private List<UInt32> GetIdsList(IEnumerable<CorePackage.Global.IDefinition> definitions)
        {
            var list = new List<UInt32>();

            foreach (var def in definitions)
            {
                list.Add(Factory.GetEntityID(def));
            }
            return list;
        }

        private SerializationModel.Context GetContextFrom(CorePackage.Entity.Context definition)
        {
            return new SerializationModel.Context
            {
                Children = GetIdsList(definition.GetEntities().Values)
            };
        }

        private SerializationModel.DataType GetDataTypeFrom(CorePackage.Entity.DataType definition)
        {
            SerializationModel.DataType.WHICH typeid;

            if (definition == Scalar.Boolean)
                typeid = SerializationModel.DataType.WHICH.BOOLEAN;
            else if (definition == Scalar.Integer)
                typeid = SerializationModel.DataType.WHICH.INTEGER;
            else if (definition == Scalar.Floating)
                typeid = SerializationModel.DataType.WHICH.FLOATING;
            else if (definition == Scalar.Character)
                typeid = SerializationModel.DataType.WHICH.CHARACTER;
            else if (definition == Scalar.String)
                typeid = SerializationModel.DataType.WHICH.STRING;
            else if (definition == DictType.Instance)
                typeid = SerializationModel.DataType.WHICH.DICT;
            else if (definition == AnyType.Instance)
                typeid = SerializationModel.DataType.WHICH.ANY;
            else
                throw new InvalidOperationException("Entity cannot be serialized as data type");

            return new SerializationModel.DataType
            {
                TypeID = typeid
            };
        }

        private SerializationModel.EnumType GetEnumFrom(EnumType definition)
        {
            var values = new Dictionary<string, string>();

            foreach (var val in definition.Values)
            {
                values[val.Key] = Newtonsoft.Json.JsonConvert.SerializeObject(val.Value);
            }

            return new SerializationModel.EnumType
            {
                StoredType = Factory.GetEntityID(definition.Stored),
                Values = values
            };
        }

        private SerializationModel.Function GetFunctionFrom(CorePackage.Entity.Function definition)
        {
            var instrs = new List<SerializationModel.Instruction>();

            foreach (var instr in definition.Instructions)
            {
                var toadd = new SerializationModel.Instruction();

                //todo reverse instruction args
                instrs.Add(toadd);
            }

            return new SerializationModel.Function
            {
                Children = GetIdsList(definition.GetEntities().Values),
                Parameters = definition.Parameters.Keys.ToList(),
                Returns = definition.Returns.Keys.ToList()
            };
        }

        #endregion

        public void LoadFrom(string filename)
        {

        }

        #region LoadHelpers
        #endregion
    }
}
