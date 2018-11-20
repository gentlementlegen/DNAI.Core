using CorePackage.Entity;
using CorePackage.Entity.Type;
using CorePackage.Execution;
using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl
{
    public class CoreSerializer
    {
        public EntityFactory Factory { private get; set; }

        private Controller Controller { get; set; } = new Controller();

        private Dictionary<IDefinition, UInt32> NewIds { get; set; } = new Dictionary<IDefinition, uint>();

        public Dictionary<uint, uint> FileIdToCoreId { get; set; }

        private CoreFile File { get; set; } = new CoreFile
        {
            MagicNumber = EntityFactory.MagicNumber,
            Version = Controller.Version,
            Entities = new List<SerializationModel.Entity>()
        };

        public void SaveTo(string filename)
        {
            var entities = new List<dynamic>();

            File.Entities.Clear();
            NewIds.Clear();

            // we need to define all the ids before serializing
            foreach (var defPair in Factory.Definitions)
            {
                IDeclarator parent = defPair.Value.Parent;

                File.Entities.Add(new SerializationModel.Entity
                {
                    Id = (uint)File.Entities.Count,
                    Name = defPair.Value.Name,
                    Type = Factory.GetEntityType(defPair.Key),
                    Visibility = parent == null ? EntityFactory.VISIBILITY.PUBLIC : (EntityFactory.VISIBILITY)parent.GetVisibilityOf(defPair.Value.Name)
                });
                NewIds[defPair.Value] = File.Entities.Last().Id;
            }

            // then we just have to get each data individually
            foreach (IDefinition definition in NewIds.Keys)
            {
                if (entities.Count == 9)
                {
                    Console.WriteLine("On debug ici");
                }

                dynamic entity = GetSerializableEntityFrom(definition);

                if (entity == null)
                {
                    throw new InvalidOperationException($"Cannot determine the type of entity to serialize {entity.Type}");
                }

                entities.Add(entity);
            }

            // finally we persist the data into a file
            using (var saveFile = new StreamWriter(filename))
            {
                BinarySerializer.Serializer.Serialize(File, saveFile.BaseStream);
                foreach (var entity in entities)
                {
                    BinarySerializer.Serializer.Serialize(entity, saveFile.BaseStream);
                }
            }
        }

        #region SaveHelpers

        #region utils

        private UInt32 GetIdOf(IDefinition definition)
        {
            return NewIds[definition];
        }

        private List<UInt32> GetIdsList(IEnumerable<CorePackage.Global.IDefinition> definitions)
        {
            var list = new List<UInt32>();

            foreach (var def in definitions)
            {
                list.Add(GetIdOf(def));
            }
            return list;
        }

        private string GetJsonValue(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        #endregion

        private SerializationModel.Context GetContextFrom(Context definition)
        {
            return new SerializationModel.Context
            {
                Children = GetIdsList(definition.GetEntities().Values)
            };
        }

        private SerializationModel.DataType GetDataTypeFrom(DataType definition)
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
            else if (definition == Matrix.Instance)
                typeid = SerializationModel.DataType.WHICH.MATRIX;
            else if (definition == Ressource.Instance)
                typeid = SerializationModel.DataType.WHICH.RESSOURCE;
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
                values[val.Key] = GetJsonValue(val.Value.Value);
            }

            return new SerializationModel.EnumType
            {
                StoredType = GetIdOf(definition.Stored),
                Values = values
            };
        }

        private SerializationModel.Function GetFunctionFrom(Function definition)
        {
            var instrs = new List<SerializationModel.Instruction>();
            var instructionsIDs = new Dictionary<CorePackage.Execution.Instruction, uint>();
            var dataLinks = new List<SerializationModel.DataLink>();
            var flowLinks = new List<SerializationModel.FlowLink>();
            int entryPoint = -1;

            //fill global instruction array
            foreach (var instr in definition.Instructions)
            {
                var toadd = new SerializationModel.Instruction
                {
                    InstructionType = InstructionFactory.GetInstructionType(instr),
                    Construction = new List<uint>(),
                    InputValues = new Dictionary<string, string>()
                };

                foreach (IDefinition def in instr.ConstructionList)
                {
                    toadd.Construction.Add(Factory.GetEntityID(def));
                }

                foreach (var input in instr.Inputs)
                {
                    if (input.Value.IsValueSet && !input.Value.IsLinked)
                    {
                        toadd.InputValues[input.Key] = GetJsonValue(input.Value.Value);
                    }
                }

                if (instr == definition.EntryPoint)
                {
                    entryPoint = instrs.Count;
                }

                instructionsIDs[instr] = (uint)instrs.Count;
                instrs.Add(toadd);
            }

            //fill links instruction arrays
            foreach (var instr in definition.Instructions)
            {
                uint id = instructionsIDs[instr];

                //fill flow links
                if (instr is ExecutionRefreshInstruction exec)
                {
                    uint i = 0;

                    foreach (ExecutionRefreshInstruction exeInstr in exec.ExecutionPins)
                    {
                        if (exeInstr != null)
                        {
                            flowLinks.Add(new SerializationModel.FlowLink
                            {
                                OutflowInstructionID = id,
                                OutflowPin = i++,
                                InflowInstructionID = instructionsIDs[exeInstr]
                            });
                        }
                    }
                }

                //fill data links
                foreach (var input in instr.Inputs)
                {
                    if (input.Value.IsLinked)
                    {
                        dataLinks.Add(new SerializationModel.DataLink
                        {
                            InputInstructionID = id,
                            Input = input.Key,
                            OutputInstructionID = instructionsIDs[input.Value.Link.Instruction],
                            Output = input.Value.Link.Output
                        });
                    }
                }
            }

            return new SerializationModel.Function
            {
                Children = GetIdsList(definition.GetEntities().Values),
                Parameters = definition.Parameters.Keys.ToList(),
                Returns = definition.Returns.Keys.ToList(),
                Instructions = instrs,
                EntryPointIndex = entryPoint,
                DataLinks = dataLinks,
                FlowLinks = flowLinks
            };
        }

        private SerializationModel.ListType GetListTypeFrom(ListType definition)
        {
            return new SerializationModel.ListType
            {
                StoredType = Factory.GetEntityID(definition.Stored)
            };
        }

        private SerializationModel.ObjectType GetObjectTypeFrom(ObjectType definition)
        {
            var attrs = new Dictionary<string, UInt32>();

            foreach (KeyValuePair<string, CorePackage.Global.IDefinition> attr in definition.GetAttributes())
            {
                attrs.Add(attr.Key, Factory.GetEntityID(attr.Value));
            }

            return new SerializationModel.ObjectType
            {
                Children = GetIdsList(definition.GetEntities().Values),
                Attributes = attrs
            };
        }

        private SerializationModel.Variable GetVariableFrom(Variable definition)
        {
            return new SerializationModel.Variable
            {
                Type = Factory.GetEntityID(definition.Type),
                Value = GetJsonValue(definition.Value)
            };
        }

        private dynamic GetSerializableEntityFrom(IDefinition definition)
        {
            uint definitionId = GetIdOf(definition);
            int index = (int)definitionId;
            SerializationModel.Entity entity = File.Entities[index];

            switch (entity.Type)
            {
                case EntityFactory.ENTITY.CONTEXT:
                    return GetContextFrom((Context)definition);
                case EntityFactory.ENTITY.DATA_TYPE:
                    return GetDataTypeFrom((DataType)definition);
                case EntityFactory.ENTITY.ENUM_TYPE:
                    return GetEnumFrom((EnumType)definition);
                case EntityFactory.ENTITY.FUNCTION:
                    return GetFunctionFrom((Function)definition);
                case EntityFactory.ENTITY.LIST_TYPE:
                    return GetListTypeFrom((ListType)definition);
                case EntityFactory.ENTITY.OBJECT_TYPE:
                    return GetObjectTypeFrom((ObjectType)definition);
                case EntityFactory.ENTITY.VARIABLE:
                    return GetVariableFrom((Variable)definition);
                default:
                    return null;
            }
        }

        #endregion

        public Controller LoadFrom(string filename)
        {
            var entities = new List<dynamic>();

            Controller.Reset();
            File.Entities.Clear();
            NewIds.Clear();

            FileIdToCoreId = new Dictionary<uint, uint>
            {
                { 0, (uint)EntityFactory.BASE_ID.GLOBAL_CTX },
                { 1, (uint)EntityFactory.BASE_ID.BOOLEAN_TYPE },
                { 2, (uint)EntityFactory.BASE_ID.INTEGER_TYPE },
                { 3, (uint)EntityFactory.BASE_ID.FLOATING_TYPE },
                { 4, (uint)EntityFactory.BASE_ID.CHARACTER_TYPE },
                { 5, (uint)EntityFactory.BASE_ID.STRING_TYPE },
                { 6, (uint)EntityFactory.BASE_ID.DICT_TYPE },
                { 7, (uint)EntityFactory.BASE_ID.ANY_TYPE },
                { 8, (uint)EntityFactory.BASE_ID.MATRIX_TYPE },
                { 9, (uint)EntityFactory.BASE_ID.RESSOURCE_TYPE }
            };

            using (var loadFile = new StreamReader(filename))
            {
                //récupérer les entites du fichier
                File = BinarySerializer.Serializer.Deserialize<CoreFile>(loadFile.BaseStream);

                if (File.MagicNumber != EntityFactory.MagicNumber)
                {
                    throw new FileLoadException("Trying to load an invalid or corrupted dnai file");
                }

                //vérification de la version
                if (File.Version > Controller.Version)
                {
                    throw new FileLoadException($"Trying to load a file compiled from DNAI.Core {File.Version.Value}: update your DNAI.Core");
                }

                //récupération des données des entités
                foreach (var entity in File.Entities)
                {
                    var serialized = GetSerializedEntityFrom(entity, loadFile.BaseStream);

                    if (serialized == null)
                    {
                        throw new FileLoadException("Trying to load an invalid or corrupted dnai file");
                    }

                    entities.Add(serialized);
                }
            }

            //déclaration de toutes les entités
            for (int i = 0; i < entities.Count; i++)
            {
                //si l'entité est un contexte
                if (entities[i] is SerializationModel.Context context)
                {
                    SerializationModel.Entity parent = File.Entities[i];

                    foreach (uint childID in context.Children)
                    {
                        SerializationModel.Entity child = File.Entities[(int)childID];

                        if (child.Type != EntityFactory.ENTITY.DATA_TYPE)
                        {
                            FileIdToCoreId[child.Id] = Controller.Declare(child.Type, FileIdToCoreId[parent.Id], child.Name, child.Visibility);
                        }
                    }
                }
            }

            var funcs = new Dictionary<SerializationModel.Entity, SerializationModel.Function>();
            
            //réplication des toutes les entités (sauf les instructions)
            for (int i = 0; i < entities.Count; i++)
            {
                SerializationModel.Entity entity = File.Entities[i];
                dynamic data = entities[i];

                ReplicateEntityFrom(entity, data);

                if (data is SerializationModel.Function func)
                {
                    funcs[entity] = func;
                }
            }

            //réplication des instructions
            foreach (var func in funcs)
            {
                uint funcId = FileIdToCoreId[func.Key.Id];

                ReplicateInstructionsFrom(funcId, func.Value);
            }

            return Controller;
        }

        #region LoadHelpers

        #region utils

        private uint GetControllerID(uint fileID)
        {
            return FileIdToCoreId[File.Entities[(int)fileID].Id];
        }

        private List<uint> GetControllerIdsList(List<uint> fileIdsList)
        {
            var ids = new List<uint>();

            foreach (uint id in fileIdsList)
            {
                ids.Add(GetControllerID(id));
            }
            return ids;
        }

        #endregion

        private void ReplicateEnumFrom(uint entityId, SerializationModel.EnumType data)
        {
            Controller.SetEnumerationType(entityId, GetControllerID(data.StoredType));

            foreach (KeyValuePair<string, string> value in data.Values)
            {
                Controller.SetEnumerationJSONValue(entityId, value.Key, value.Value);
            }
        }

        private void ReplicateFunctionFrom(uint entityId, SerializationModel.Function data)
        {
            foreach (string param in data.Parameters)
            {
                Controller.SetFunctionParameter(entityId, param);
            }

            foreach (string ret in data.Returns)
            {
                Controller.SetFunctionReturn(entityId, ret);
            }
        }

        private void ReplicateInstructionsFrom(uint entityId, SerializationModel.Function data)
        {
            var instructionIDs = new Dictionary<uint, uint>();
            uint i = 0;

            //replicate instructions
            foreach (SerializationModel.Instruction instr in data.Instructions)
            {
                uint instrId = Controller.AddInstruction(entityId, instr.InstructionType, GetControllerIdsList(instr.Construction));
                
                //replicate input values
                foreach (var inputValue in instr.InputValues)
                {
                    Controller.SetInstructionInputJSONValue(
                        entityId,
                        instrId,
                        inputValue.Key,
                        inputValue.Value
                    );
                }

                instructionIDs[i++] = instrId;
            }

            //replicate datalinks
            foreach (SerializationModel.DataLink dataLink in data.DataLinks)
            {
                Controller.LinkInstructionData(
                    entityId,
                    instructionIDs[dataLink.OutputInstructionID],
                    dataLink.Output,
                    instructionIDs[dataLink.InputInstructionID],
                    dataLink.Input
                );
            }

            //replicate flowlinks
            foreach (SerializationModel.FlowLink flowLink in data.FlowLinks)
            {
                Controller.LinkInstructionExecution(
                    entityId,
                    instructionIDs[flowLink.OutflowInstructionID],
                    flowLink.OutflowPin,
                    instructionIDs[flowLink.InflowInstructionID]
                );
            }

            if (data.EntryPointIndex != -1)
            {
                Controller.SetFunctionEntryPoint(entityId, instructionIDs[(uint)data.EntryPointIndex]);
            }
        }

        private void ReplicateListTypeFrom(uint entityId, SerializationModel.ListType data)
        {
            Controller.SetListType(entityId, GetControllerID(data.StoredType));
        }

        private void ReplicateObjectTypeFrom(uint entityId, SerializationModel.ObjectType data)
        {
            foreach (var attr in data.Attributes)
            {
                Controller.AddClassAttribute(entityId, attr.Key, GetControllerID(attr.Value), EntityFactory.VISIBILITY.PUBLIC);
            }
        }

        private void ReplicateVariableFrom(uint entityId, SerializationModel.Variable data)
        {
            Controller.SetVariableType(entityId, GetControllerID(data.Type));
            Controller.SetVariableJSONValue(entityId, data.Value);
        }
        
        private void ReplicateEntityFrom(SerializationModel.Entity entity, dynamic data)
        {
            uint entityId = FileIdToCoreId[entity.Id];

            switch (entity.Type)
            {
                case EntityFactory.ENTITY.VARIABLE:
                    ReplicateVariableFrom(entityId, data);
                    break;
                case EntityFactory.ENTITY.FUNCTION:
                    ReplicateFunctionFrom(entityId, data);
                    break;
                case EntityFactory.ENTITY.ENUM_TYPE:
                    ReplicateEnumFrom(entityId, data);
                    break;
                case EntityFactory.ENTITY.OBJECT_TYPE:
                    ReplicateObjectTypeFrom(entityId, data);
                    break;
                case EntityFactory.ENTITY.LIST_TYPE:
                    ReplicateListTypeFrom(entityId, data);
                    break;
            }
        }

        private dynamic GetSerializedEntityFrom(SerializationModel.Entity entity, Stream file)
        {
            switch (entity.Type)
            {
                case EntityFactory.ENTITY.CONTEXT:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.Context>(file);
                case EntityFactory.ENTITY.VARIABLE:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.Variable>(file);
                case EntityFactory.ENTITY.FUNCTION:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.Function>(file);
                case EntityFactory.ENTITY.DATA_TYPE:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.DataType>(file);
                case EntityFactory.ENTITY.ENUM_TYPE:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.EnumType>(file);
                case EntityFactory.ENTITY.OBJECT_TYPE:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.ObjectType>(file);
                case EntityFactory.ENTITY.LIST_TYPE:
                    return BinarySerializer.Serializer.Deserialize<SerializationModel.ListType>(file);
                default:
                    return null;
            }
        }

        #endregion
    }
}