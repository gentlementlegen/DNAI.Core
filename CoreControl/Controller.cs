using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreControl
{
    /// <summary>
    /// Class that is used to control the core model without having to access to any type of it
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Entity factory used to manage entities
        /// </summary>
        private EntityFactory entity_factory = new EntityFactory();
        
        /// <summary>
        /// Resets the controller state to initial values.
        /// </summary>
        public void Reset()
        {
            entity_factory = new EntityFactory();
        }

        public void SaveTo(string filename)
        {
            CoreSerializer serializer = new CoreSerializer
            {
                Factory = entity_factory
            };

            serializer.SaveTo(filename);
        }

        public void LoadFrom(string filename)
        {
            CoreSerializer serializer = new CoreSerializer
            {
                Factory = new EntityFactory()
            };

            merge(serializer.LoadFrom(filename));
        }

        public UInt32 FindEntity(string path)
        {
            String[] names = path.Substring(1).Split('/');
            CorePackage.Global.IDefinition cwe = entity_factory.Find(EntityFactory.BASE_ID.GLOBAL_CTX);

            foreach (string name in names)
            {
                if (cwe is CorePackage.Global.IDeclarator decl)
                {
                    cwe = decl.Find(name);
                }
                else
                {
                    throw new KeyNotFoundException($"Cannot access to entity {name} from {cwe.Name}: Not a declarator");
                }
            }
            return entity_factory.GetEntityID(cwe);
        }

        /// <summary>
        /// Will declare an entity in a container with a specific name and visibility
        /// </summary>
        /// <param name="entity_type">Type of the entity to declare</param>
        /// <param name="containerID">Identifier of the container in which declare the entity</param>
        /// <param name="name">Name of the declared entity</param>
        /// <param name="visibility">Visibility of the declared entity</param>
        /// <returns>Identifier of the freshly declared entity</returns>
        public UInt32 Declare(EntityFactory.ENTITY entity_type, UInt32 containerID, string name, EntityFactory.VISIBILITY visibility)
        {
            return entity_factory.Declare(entity_type, containerID, name, visibility);
        }

        /// <summary>
        /// Remove an entity declared in a container
        /// </summary>
        /// <param name="entity_type">Type of entity contained in the declarator</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="name">Name of the entity to remove in the container</param>
        /// <returns>List of all removed entities' id</returns>
        public List<UInt32> Remove(UInt32 containerID, string name)
        {
            return entity_factory.Remove(containerID, name);
        }

        /// <summary>
        /// Rename an entity declared in a container
        /// </summary>
        /// <param name="entity_type">Type of the entity contained in the declarator</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="lastName">Current name of the entity</param>
        /// <param name="newName">New name to set</param>
        public void Rename(UInt32 containerID, string lastName, string newName)
        {
            entity_factory.Rename(containerID, lastName, newName);
        }

        /// <summary>
        /// Move an entity from a specific container to another
        /// </summary>
        /// <param name="entity_type">Type of the entity contained in the declarator</param>
        /// <param name="fromID">Identifier of the entity declared in the container</param>
        /// <param name="toID">Identifier of the declarator in which move the entity</param>
        /// <param name="name">Name of the entity to move</param>
        public void Move(UInt32 fromID, UInt32 toID, string name)
        {
            entity_factory.Move(fromID, toID, name);
        }

        /// <summary>
        /// Change an entity visibility declared in a specific container
        /// </summary>
        /// <param name="entity_type">Type of the entity to change visibility</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="name">Name of the declared entity in the container</param>
        /// <param name="newVisi">New visibility of the declared entity</param>
        public void ChangeVisibility(UInt32 containerID, string name, EntityFactory.VISIBILITY newVisi)
        {
            entity_factory.ChangeVisibility(containerID, name, newVisi);
        }

        public List<EntityFactory.Entity> GetEntities(UInt32 containerID)
        {
            List<EntityFactory.Entity> toret = new List<EntityFactory.Entity>();

            CorePackage.Global.IDeclarator decl = entity_factory.GetDeclaratorOf(containerID);

            foreach (KeyValuePair<string, CorePackage.Global.IDefinition> curr in decl.GetEntities(CorePackage.Global.AccessMode.EXTERNAL))
            {
                UInt32 id = entity_factory.GetEntityID(curr.Value);

                toret.Add(new EntityFactory.Entity
                {
                    Id = id,
                    Name = curr.Value.Name,
                    Type = GetEntityType(id)
                });
            }
            return toret;
        }

        /// <summary>
        /// Returns the type of a given entity in order to use good methods on it
        /// </summary>
        /// <param name="entityId">Identifier of the entity to retreive the type</param>
        /// <returns>Type of the given entity</returns>
        public EntityFactory.ENTITY GetEntityType(UInt32 entityId)
        {
            return entity_factory.GetEntityType(entityId);
        }

        /// <summary>
        /// Will expose all externals entities of a specific type in a given container
        /// </summary>
        /// <param name="entities_type">Type of the entities to expose</param>
        /// <param name="containerID"></param>
        /// <returns></returns>
        public List<EntityFactory.Entity> GetEntitiesOfType(EntityFactory.ENTITY entities_type, UInt32 containerID)
        {
            return entity_factory.GetEntitiesOfType(entities_type, containerID);
        }

        /// <summary>
        /// Set a specific value to a variable
        /// </summary>
        /// <param name="variableID">Identifier of the variable</param>
        /// <param name="value">Value to set to the variable</param>
        public void SetVariableValue(UInt32 variableID, dynamic value)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Variable>(variableID).Value = value;
        }
        
        /// <summary>
        /// Get a variable value
        /// </summary>
        /// <param name="variableID">Identifier of the variable from which retreive the value</param>
        /// <returns>Value of the variable identified by the given id</returns>
        public dynamic GetVariableValue(UInt32 variableID)
        {
            return entity_factory.FindDefinitionOfType<CorePackage.Entity.Variable>(variableID).Value;
        }
        
        /// <summary>
        /// Set a specific type to a variable
        /// </summary>
        /// <param name="variableID">Identifier of the variable to which set the type</param>
        /// <param name="typeID">Identifier of the type to set</param>
        public void SetVariableType(UInt32 variableID, UInt32 typeID)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Variable>(variableID).Type = entity_factory.FindDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        /// <summary>
        /// Returns given variable type
        /// </summary>
        /// <param name="variableID">Identifier of the variable on which retreive type</param>
        /// <returns>Identifier of the variable type</returns>
        public UInt32 GetVariableType(UInt32 variableID)
        {
            return entity_factory.GetEntityID(entity_factory.FindDefinitionOfType<CorePackage.Entity.Variable>(variableID).Type);
        }

        /// <summary>
        /// Set a parent context to a specific context
        /// </summary>
        /// <param name="contextID">Identifier of the context to which set the parent</param>
        /// <param name="parentID">Identifier of the parent context to set</param>
        public void SetContextParent(UInt32 contextID, UInt32 parentID)
        {
            //entity_factory.FindDefinitionOfType<CorePackage.Entity.Context>(contextID).SetParent(entity_factory.FindDefinitionOfType<CorePackage.Entity.Context>(parentID));
        }

        public dynamic InstantiateType(UInt32 dataTypeID)
        {
            return entity_factory.FindDefinitionOfType<CorePackage.Entity.DataType>(dataTypeID).Instantiate();
        }

        /// <summary>
        /// Set a type stored in a specific enumeration
        /// </summary>
        /// <param name="enumID">Identifier of the enumeration to which set stored type</param>
        /// <param name="typeID">Identifier of the type to set into the enum</param>
        public void SetEnumerationType(UInt32 enumID, UInt32 typeID)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).Stored = entity_factory.FindDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        /// <summary>
        /// Returns the highest context name.
        /// </summary>
        /// <returns></returns>
        public string GetMainContextName()
        {
            return GetEntities(GetIds(EntityFactory.EntityType.CONTEXT | EntityFactory.EntityType.PUBLIC))?.Find(x => !string.IsNullOrEmpty(x.Name))?.Name;
        }

        /// <summary>
        /// Set an enumeration value
        /// </summary>
        /// <param name="enumID">Identifier of specific enumeration</param>
        /// <param name="name">Name of the enum value to set</param>
        /// <param name="value">Value to set in the enum</param>
        public void SetEnumerationValue(UInt32 enumID, string name, dynamic value)
        {
            CorePackage.Entity.Type.EnumType to_find = entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID);
            CorePackage.Entity.Variable var = new CorePackage.Entity.Variable(to_find.Stored, value);
            to_find.SetValue(name, var);
        }

        /// <summary>
        /// Get an enumeration value
        /// </summary>
        /// <param name="enumID">Identifier of a specific enumeration</param>
        /// <param name="name">Name of an enum value</param>
        /// <returns>Value to retreive</returns>
        public dynamic GetEnumerationValue(UInt32 enumID, string name)
        {
            return entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).GetValue(name).Value;
        }

        /// <summary>
        /// Returns a list of all name's values contained in the enumeration
        /// </summary>
        /// <param name="enumID">Identifier of the enumeration</param>
        /// <returns>List of given enumeration values</returns>
        public List<String> GetEnumerationValues(UInt32 enumID)
        {
            return new List<String>(entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).Values.Keys);
        }

        /// <summary>
        /// Remove a specific enum value
        /// </summary>
        /// <param name="enumID">Identifier of a specific enumeration</param>
        /// <param name="name">Name of an enum value</param>
        public void RemoveEnumerationValue(UInt32 enumID, string name)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).RemoveValue(name);
        }

        /// <summary>
        /// Add an attribute to a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="name">Name of the attribute to add</param>
        /// <param name="typeID">Identifier of the attribute type to add</param>
        /// <param name="visibility">Visibility of the attribute to add</param>
        public void AddClassAttribute(UInt32 classID, string name, UInt32 typeID, EntityFactory.VISIBILITY visibility)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).AddAttribute(name, entity_factory.FindDefinitionOfType<CorePackage.Entity.DataType>(typeID), (CorePackage.Global.AccessMode)visibility);
        }

        /// <summary>
        /// Rename an attribute in a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="lastName">Current name of the attribute to rename</param>
        /// <param name="newName">Name to set to the attribute</param>
        public void RenameClassAttribute(UInt32 classID, string lastName, string newName)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RenameAttribute(lastName, newName);
        }

        /// <summary>
        /// Remove an attribute in a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="name">Name of the attribute to remove</param>
        public void RemoveClassAttribute(UInt32 classID, string name)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RemoveAttribute(name);
        }

        public List<String> GetClassAttributes(UInt32 classID)
        {
            return entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).GetAttributes().Keys.ToList();
        }

        public UInt32 GetClassAttribute(UInt32 classID, String name)
        {
            return entity_factory.GetEntityID(entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).GetAttribute(name));
        }
        
        /// <summary>
        /// Add a member function to a class
        /// </summary>
        /// <remarks>Will declare 'this' parameter in a given function</remarks>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="funcname">Name of the method to set as member</param>
        /// <param name="visibility">Visibility of the method to add</param>
        /// <returns>Identifier of the 'this' parameter added</returns>
        public UInt32 SetClassFunctionAsMember(UInt32 classID, string funcname)
        {
            CorePackage.Entity.Type.ObjectType objtype = entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID);

            entity_factory.AddEntity(objtype.SetFunctionAsMember(funcname));

            return entity_factory.LastID;
        }

        /// <summary>
        /// Set a type to a specific list
        /// </summary>
        /// <param name="listID">Identifier of the list to which set the type</param>
        /// <param name="typeID">Identifier of the type to set in the list</param>
        public void SetListType(UInt32 listID, UInt32 typeID)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Type.ListType>(listID).Stored = entity_factory.FindDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }
        
        /// <summary>
        /// Call a specific function with specific parameters
        /// </summary>
        /// <param name="funcID">Identifier of a specific function to call</param>
        /// <param name="parameters">Dictionary of parameters to set</param>
        /// <returns>Dictionary that contains function returns' value</returns>
        public Dictionary<string, dynamic> CallFunction(UInt32 funcID, Dictionary<string, dynamic> parameters)
        {
            return entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcID).Call(parameters);
        }
        
        public void DumpFunctionInto(UInt32 funcId, string directory)
        {
            CorePackage.Entity.Function func = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcId);
            System.IO.FileStream file = new System.IO.FileStream(directory + "/" + func.Name + ".dot", System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

            if (file.CanWrite)
            {
                string dat = func.ToDotFile();

                file.Write(Encoding.ASCII.GetBytes(dat), 0, dat.Length);
                file.Close();
            }
            else
            {
                throw new InvalidOperationException("Couldn't write data in file");
            }
        }

        /// <summary>
        /// Set a function public variable as parameter
        /// </summary>
        /// <param name="funcID">Identifier of the function in which set the parameter</param>
        /// <param name="externalVarName">Name of the public variable to set as parameter</param>
        public void SetFunctionParameter(UInt32 funcID, string externalVarName)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.PARAMETER);
        }

        /// <summary>
        /// Allow user to expose function parameters
        /// </summary>
        /// <param name="funcID">Identifier of the declared function in which retreive parameters</param>
        /// <returns>List of entities that contains function parameters</returns>
        public List<EntityFactory.Entity> GetFunctionParameters(UInt32 funcID)
        {
            List<EntityFactory.Entity> externals = GetEntitiesOfType(EntityFactory.ENTITY.VARIABLE, funcID);
            List<EntityFactory.Entity> filtered = new List<EntityFactory.Entity>();
            Dictionary<String, CorePackage.Entity.Variable> parameters = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcID).Parameters;
            
            foreach (EntityFactory.Entity curr in externals)
            {
                if (parameters.ContainsKey(curr.Name))
                    filtered.Add(curr);
            }
            return filtered;
        }

        /// <summary>
        /// Set a function public variable as return
        /// </summary>
        /// <param name="funcID">Identifier of specific function</param>
        /// <param name="externalVarName">Name of an external variable to set as return</param>
        public void SetFunctionReturn(UInt32 funcID, string externalVarName)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.RETURN);
        }

        /// <summary>
        /// Allow user to expose function returns
        /// </summary>
        /// <param name="funcID">Identifier of the declared function in which retreive returns</param>
        /// <returns>List of the entities that corresponds to the returns variable in the function</returns>
        public List<EntityFactory.Entity> GetFunctionReturns(UInt32 funcID)
        {
            List<EntityFactory.Entity> externals = GetEntitiesOfType(EntityFactory.ENTITY.VARIABLE, funcID);
            List<EntityFactory.Entity> filtered = new List<EntityFactory.Entity>();
            Dictionary<String, CorePackage.Entity.Variable> parameters = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(funcID).Returns;

            foreach (EntityFactory.Entity curr in externals)
            {
                if (parameters.ContainsKey(curr.Name))
                    filtered.Add(curr);
            }
            return filtered;
        }

        /// <summary>
        /// Set an instruction as entry point in a function
        /// </summary>
        /// <param name="functionID">Identifier of a function to which set entry point</param>
        /// <param name="instruction">Identifier of an instruction in the function</param>
        public void SetFunctionEntryPoint(UInt32 functionID, UInt32 instruction)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).setEntryPoint(instruction);
        }

        /// <summary>
        /// Remove an instruction from a specific function
        /// </summary>
        /// <param name="functionID">Identifier of a specific function</param>
        /// <param name="instruction">Identifier of an instruction to remove from the function</param>
        public void RemoveFunctionInstruction(UInt32 functionID, UInt32 instruction)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).removeInstruction(instruction);
            //could be nice to return the ids that have been data unlinked
            //could be nice to return the ids that have been exec unlinked
        }

        /// <summary>
        /// Add an instruction to a specific function
        /// </summary>
        /// <param name="functionID">Identifier of a specific function</param>
        /// <param name="to_create">Type of instruction to add</param>
        /// <param name="crea_args">List of entities' identifier to send to the instruction constructor</param>
        /// <returns>The identifier of the created instruction</returns>
        public UInt32 AddInstruction(UInt32 functionID, InstructionFactory.INSTRUCTION_ID to_create, List<UInt32> crea_args)
        {
            CorePackage.Entity.Function func = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID);
            List<CorePackage.Global.IDefinition> crea_definitions = new List<CorePackage.Global.IDefinition>();

            foreach (UInt32 definitionID in crea_args)
            {
                crea_definitions.Add(entity_factory.Find(definitionID));
            }
            return func.addInstruction(InstructionFactory.CreateInstruction(to_create, crea_definitions));
        }

        /// <summary>
        /// Link an execution pin of an instruction to another in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function in which retreive the instructions</param>
        /// <param name="fromID">Instruction from which link the execution pin</param>
        /// <param name="outIndex">Index of the pin used for the link</param>
        /// <param name="toID">Instruction to link</param>
        public void LinkInstructionExecution(UInt32 functionID, UInt32 fromID, UInt32 outIndex, UInt32 toID)
        {
            CorePackage.Entity.Function func = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.LinkInstructionExecution(fromID, outIndex, toID);
            //func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(fromID).LinkTo(outIndex, func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(toID));
        }

        /// <summary>
        /// Link an input pin of an instruction to an output pin of another in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function in which retreive the instructions</param>
        /// <param name="fromID">Identifier of the instruction from which link the output</param>
        /// <param name="outputName">Name of the output to link</param>
        /// <param name="toID">Identifier of the instruction to which link to input</param>
        /// <param name="intputName">Name of the input to link</param>
        public void LinkInstructionData(UInt32 functionID, UInt32 fromID, string outputName, UInt32 toID, string inputName)
        {
            CorePackage.Entity.Function func = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.LinkInstructionData(fromID, outputName, toID, inputName);
            //func.findInstruction<CorePackage.Execution.Instruction>(toID).GetInput(intputName).LinkTo(func.findInstruction<CorePackage.Execution.Instruction>(fromID), outputName);
        }

        /// <summary>
        /// Set an input value of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function from which retreive the instruction</param>
        /// <param name="instruction">Identifier of the instruction that contains the input to set</param>
        /// <param name="inputname">Name of the input to which set the value</param>
        /// <param name="inputValue">Value to set to the input</param>
        public void SetInstructionInputValue(UInt32 functionID, UInt32 instruction, string inputname, dynamic inputValue)
        {
            CorePackage.Entity.Function func = entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.Instruction>(instruction).SetInputValue(inputname, inputValue);
        }

        /// <summary>
        /// Unlink an execution pin of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function that contains the instruction to unlink</param>
        /// <param name="instruction">Identifier of the instruction to unlink in the function</param>
        /// <param name="outIndex">Index of the pin to unlink in the instruction</param>
        public void UnlinkInstructionFlow(UInt32 functionID, UInt32 instruction, uint outIndex)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).UnlinkInstructionFlow(instruction, outIndex);
            //entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(instruction).Unlink(outIndex);
        }

        /// <summary>
        /// Unlink an input of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function that contains the instruction</param>
        /// <param name="instruction">Identifier of the instruction to unlink in the function</param>
        /// <param name="inputname">Name of the input to unlink</param>
        public void UnlinkInstructionInput(UInt32 functionID, UInt32 instruction, string inputname)
        {
            entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).UnlinkInstructionInput(instruction, inputname);
            //entity_factory.FindDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.Instruction>(instruction).GetInput(inputname).Unlink();
        }

        /// <summary>
        /// Retrieves ids contained in the controller.
        /// </summary>
        /// <param name="flags">Filters for the request.</param>
        /// <returns>The corresponding ID list.</returns>
        public List<uint> GetIds(EntityFactory.EntityType flags = EntityFactory.EntityType.ALL)
        {
            return entity_factory.GetIds(flags);
        }

        /// <summary>
        /// Get an entity from its id
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <returns>The entity corresponding to the id</returns>
        public EntityFactory.Entity GetEntity(UInt32 id)
        {
            CorePackage.Global.IDefinition def = entity_factory.Find(id);

            return new EntityFactory.Entity
            {
                Id = id,
                Name = def.Name,
                Type = GetEntityType(id)
            };
        }

        /// <summary>
        /// Get a list of entities corresponding to the given ids.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<EntityFactory.Entity> GetEntities(List<UInt32> ids)
        {
            if (ids == null) return null;
            var ret = new List<EntityFactory.Entity>();

            foreach (var id in ids)
            {
                CorePackage.Global.IDefinition def = entity_factory.Find(id);
                ret.Add(new EntityFactory.Entity { Id = id, Name = def.Name, Type = GetEntityType(id) });
            }
            return ret;
        }

        public void merge(Controller controller)
        {
            entity_factory.merge(controller.entity_factory);
        }
    }
}