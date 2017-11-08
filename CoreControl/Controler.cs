using CorePackage;
using System;
using System.Collections.Generic;

namespace CoreControl
{
    /// <summary>
    /// Class that is used to control the core model without having to access to any type of it
    /// </summary>
    public class Controler
    {
        /// <summary>
        /// Entity factory used to manage entities
        /// </summary>
        private EntityFactory entity_factory = new EntityFactory();

        //on entity removed

        //on instruction unlinked flow

        //on instruction unlinked data

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
        
        /// <summary>
        /// Associates a EntityFactory.declare function to a specific ENTITY key
        /// </summary>
        static private readonly Dictionary<ENTITY, Func<EntityFactory, UInt32, string, VISIBILITY, UInt32>> declarators = new Dictionary<ENTITY, Func<EntityFactory, uint, string, VISIBILITY, uint>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Context, CorePackage.Global.IContext>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Variable>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Function>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.ENUM_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Type.EnumType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.OBJECT_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Type.ObjectType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
                }
            },
            {
                ENTITY.LIST_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY visibility) =>
                {
                    return factory.declare<CorePackage.Entity.Type.ListType, CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)visibility);
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
        public UInt32 declare(ENTITY to_declare, UInt32 containerID, string name, VISIBILITY visibility)
        {
            if (declarators.ContainsKey(to_declare))
                return declarators[to_declare].Invoke(entity_factory, containerID, name, visibility);
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
                    return factory.remove<CorePackage.Global.IContext>(containerID, name);
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.remove<CorePackage.Entity.Variable>(containerID, name);
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.remove<CorePackage.Entity.Function>(containerID, name);
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string name) =>
                {
                    return factory.remove<CorePackage.Entity.DataType>(containerID, name);
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
        public List<UInt32> remove(ENTITY to_remove, UInt32 containerID, string name)
        {
            if (!erasers.ContainsKey(to_remove))
                throw new KeyNotFoundException("No such eraser for ENTITY: " + to_remove.ToString());
            return erasers[to_remove].Invoke(entity_factory, containerID, name);
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
                    factory.rename<CorePackage.Global.IContext>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.rename<CorePackage.Entity.Variable>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.rename<CorePackage.Entity.Function>(containerID, lastName, newName);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string lastName, string newName) =>
                {
                    factory.rename<CorePackage.Entity.DataType>(containerID, lastName, newName);
                    return true;
                }
            }
        };

        /// <summary>
        /// Rename an entity declared in a container
        /// </summary>
        /// <param name="to_rename">Type of the entity contained in the declarator</param>
        /// <param name="containerID">Identifier of the container in which entity is declared</param>
        /// <param name="lastName">Current name of the entity</param>
        /// <param name="newName">New name to set</param>
        public void rename(ENTITY to_rename, UInt32 containerID, string lastName, string newName)
        {
            if (!renamers.ContainsKey(to_rename))
                throw new KeyNotFoundException("No such renamer for ENTITY: " + to_rename.ToString());
            renamers[to_rename].Invoke(entity_factory, containerID, lastName, newName);
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
                    factory.move<CorePackage.Global.IContext>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.move<CorePackage.Entity.Variable>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.move<CorePackage.Entity.Function>(fromID, toID, name);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 fromID, UInt32 toID, string name) =>
                {
                    factory.move<CorePackage.Entity.DataType>(fromID, toID, name);
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
        public void move(ENTITY to_move, UInt32 fromID, UInt32 toID, string name)
        {
            if (!movers.ContainsKey(to_move))
                throw new KeyNotFoundException("No such mover for ENTITY: " + to_move.ToString());
            movers[to_move].Invoke(entity_factory, fromID, toID, name);
        }

        /// <summary>
        /// Associates an EntityFactory.changeVisibility function to a specific ENTITY key
        /// </summary>
        Dictionary<ENTITY, Func<EntityFactory, UInt32, string, VISIBILITY, bool>> visi_modifiers = new Dictionary<ENTITY, Func<EntityFactory, uint, string, VISIBILITY, bool>>
        {
            {
                ENTITY.CONTEXT,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.changeVisibility<CorePackage.Global.IContext>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.VARIABLE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.changeVisibility<CorePackage.Entity.Variable>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.FUNCTION,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.changeVisibility<CorePackage.Entity.Function>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
                    return true;
                }
            },
            {
                ENTITY.DATA_TYPE,
                (EntityFactory factory, UInt32 containerID, string name, VISIBILITY newVisi) =>
                {
                    factory.changeVisibility<CorePackage.Entity.DataType>(containerID, name, (CorePackage.Global.AccessMode)newVisi);
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
        public void changeVisibility(ENTITY to_change_visi, UInt32 containerID, string name, VISIBILITY newVisi)
        {
            if (!visi_modifiers.ContainsKey(to_change_visi))
                throw new KeyNotFoundException("No such visibility modifier for ENTITY: " + to_change_visi.ToString());
            visi_modifiers[to_change_visi].Invoke(entity_factory, containerID, name, newVisi);
        }

        /// <summary>
        /// Set a specific value to a variable
        /// </summary>
        /// <param name="variableID">Identifier of the variable</param>
        /// <param name="value">Value to set to the variable</param>
        public void setVariableValue(UInt32 variableID, dynamic value)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Variable>(variableID).Value = value;
        }

        /// <summary>
        /// Set a specific type to a variable
        /// </summary>
        /// <param name="variableID">Identifier of the variable to which set the type</param>
        /// <param name="typeID">Identifier of the type to set</param>
        public void setVariableType(UInt32 variableID, UInt32 typeID)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Variable>(variableID).Type = entity_factory.findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        /// <summary>
        /// Get a variable value
        /// </summary>
        /// <param name="variableID">Identifier of the variable from which retreive the value</param>
        /// <returns>Value of the variable identified by the given id</returns>
        public dynamic getVariableValue(UInt32 variableID)
        {
            return entity_factory.findDefinitionOfType<CorePackage.Entity.Variable>(variableID).Value;
        }

        /// <summary>
        /// Set a parent context to a specific context
        /// </summary>
        /// <param name="contextID">Identifier of the context to which set the parent</param>
        /// <param name="parentID">Identifier of the parent context to set</param>
        public void setContextParent(UInt32 contextID, UInt32 parentID)
        {
            entity_factory.findDefinitionOfType<CorePackage.Global.IContext>(contextID).SetParent(entity_factory.findDefinitionOfType<CorePackage.Global.IContext>(parentID));
        }

        /// <summary>
        /// Set a type stored in a specific enumeration
        /// </summary>
        /// <param name="enumID">Identifier of the enumeration to which set stored type</param>
        /// <param name="typeID">Identifier of the type to set into the enum</param>
        public void setEnumerationType(UInt32 enumID, UInt32 typeID)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).Stored = entity_factory.findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        /// <summary>
        /// Set an enumeration value
        /// </summary>
        /// <param name="enumID">Identifier of specific enumeration</param>
        /// <param name="name">Name of the enum value to set</param>
        /// <param name="value">Value to set in the enum</param>
        public void setEnumerationValue(UInt32 enumID, string name, dynamic value)
        {
            CorePackage.Entity.Type.EnumType to_find = entity_factory.findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID);
            CorePackage.Entity.Variable var = new CorePackage.Entity.Variable(to_find.Stored, value);
            to_find.SetValue(name, var);
        }

        /// <summary>
        /// Get an enumeration value
        /// </summary>
        /// <param name="enumID">Identifier of a specific enumeration</param>
        /// <param name="name">Name of an enum value</param>
        /// <returns>Value to retreive</returns>
        public dynamic getEnumerationValue(UInt32 enumID, string name)
        {
            return entity_factory.findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).GetValue(name).Value;
        }

        /// <summary>
        /// Remove a specific enum value
        /// </summary>
        /// <param name="enumID">Identifier of a specific enumeration</param>
        /// <param name="name">Name of an enum value</param>
        public void removeEnumerationValue(UInt32 enumID, string name)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).RemoveValue(name);
        }

        /// <summary>
        /// Add an attribute to a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="name">Name of the attribute to add</param>
        /// <param name="typeID">Identifier of the attribute type to add</param>
        /// <param name="visibility">Visibility of the attribute to add</param>
        public void addClassAttribute(UInt32 classID, string name, UInt32 typeID, VISIBILITY visibility)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).AddAttribute(name, entity_factory.findDefinitionOfType<CorePackage.Entity.DataType>(typeID), (CorePackage.Global.AccessMode)visibility);
        }

        /// <summary>
        /// Rename an attribute in a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="lastName">Current name of the attribute to rename</param>
        /// <param name="newName">Name to set to the attribute</param>
        public void renameClassAttribute(UInt32 classID, string lastName, string newName)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RenameAttribute(lastName, newName);
        }

        /// <summary>
        /// Remove an attribute in a class
        /// </summary>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="name">Name of the attribute to remove</param>
        public void removeClassAttribute(UInt32 classID, string name)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RemoveAttribute(name);
        }

        /// <summary>
        /// Add a member function to a class
        /// </summary>
        /// <remarks>Will declare a function where the first argument is 'this' reference</remarks>
        /// <param name="classID">Identifier of a specific class</param>
        /// <param name="name">Name of the method to add</param>
        /// <param name="visibility">Visibility of the method to add</param>
        /// <returns>Identifier of the new function added</returns>
        public UInt32 addClassMemberFunction(UInt32 classID, string name, VISIBILITY visibility)
        {
            UInt32 funcID = entity_factory.declare<CorePackage.Entity.Function>(classID, name, (CorePackage.Global.AccessMode)visibility);
            CorePackage.Entity.Type.ObjectType obj = entity_factory.findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID);
            CorePackage.Entity.Function func = ((CorePackage.Global.IDeclarator<CorePackage.Entity.Function>)obj).Find(name, (CorePackage.Global.AccessMode)visibility);

            func.Declare(new CorePackage.Entity.Variable(obj), "this", CorePackage.Global.AccessMode.EXTERNAL);
            func.SetVariableAs("this", CorePackage.Entity.Function.VariableRole.PARAMETER);
            return funcID;
        }

        /// <summary>
        /// Set a type to a specific list
        /// </summary>
        /// <param name="listID">Identifier of the list to which set the type</param>
        /// <param name="typeID">Identifier of the type to set in the list</param>
        public void setListType(UInt32 listID, UInt32 typeID)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Type.ListType>(listID).Stored = entity_factory.findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        /// <summary>
        /// Call a specific function with specific parameters
        /// </summary>
        /// <param name="funcID">Identifier of a specific function to call</param>
        /// <param name="parameters">Dictionary of parameters to set</param>
        /// <returns>Dictionary that contains function returns' value</returns>
        public Dictionary<string, dynamic> callFunction(UInt32 funcID, Dictionary<string, dynamic> parameters)
        {
            Dictionary<string, dynamic> returns = new Dictionary<string, dynamic>();

            CorePackage.Entity.Function to_call = entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(funcID);

            foreach (KeyValuePair<string, dynamic> param in parameters)
            {
                to_call.SetParameterValue(param.Key, param.Value);
            }
            to_call.Call();

            foreach (KeyValuePair<string, CorePackage.Entity.Variable> ret in to_call.Returns)
            {
                returns[ret.Key] = ret.Value.Value;
            }
            return returns;
        }

        /// <summary>
        /// Set a function public variable as parameter
        /// </summary>
        /// <param name="funcID">Identifier of the function in which set the parameter</param>
        /// <param name="externalVarName">Name of the public variable to set as parameter</param>
        public void setFunctionParameter(UInt32 funcID, string externalVarName)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.PARAMETER);
        }

        /// <summary>
        /// Set a function public variable as return
        /// </summary>
        /// <param name="funcID">Identifier of specific function</param>
        /// <param name="externalVarName">Name of an external variable to set as return</param>
        public void setFunctionReturn(UInt32 funcID, string externalVarName)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.RETURN);
        }

        /// <summary>
        /// Set an instruction as entry point in a function
        /// </summary>
        /// <param name="functionID">Identifier of a function to which set entry point</param>
        /// <param name="instruction">Identifier of an instruction in the function</param>
        public void setFunctionEntryPoint(UInt32 functionID, UInt32 instruction)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID).setEntryPoint(instruction);
        }

        /// <summary>
        /// Remove an instruction from a specific function
        /// </summary>
        /// <param name="functionID">Identifier of a specific function</param>
        /// <param name="instruction">Identifier of an instruction to remove from the function</param>
        public void removeFunctionInstruction(UInt32 functionID, UInt32 instruction)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID).removeInstruction(instruction);
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
        public UInt32 addInstruction(UInt32 functionID, InstructionFactory.INSTRUCTION_ID to_create, List<UInt32> crea_args)
        {
            CorePackage.Entity.Function func = entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID);
            List<CorePackage.Global.Definition> crea_definitions = new List<CorePackage.Global.Definition>();

            foreach (UInt32 definitionID in crea_args)
            {
                crea_definitions.Add(entity_factory.find(definitionID));
            }
            return func.addInstruction(InstructionFactory.create_instruction(to_create, crea_definitions));
        }

        /// <summary>
        /// Link an execution pin of an instruction to another in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function in which retreive the instructions</param>
        /// <param name="fromID">Instruction from which link the execution pin</param>
        /// <param name="outIndex">Index of the pin used for the link</param>
        /// <param name="toID">Instruction to link</param>
        public void linkInstructionExecution(UInt32 functionID, UInt32 fromID, UInt32 outIndex, UInt32 toID)
        {
            CorePackage.Entity.Function func = entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(fromID).LinkTo(outIndex, func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(toID));
        }

        /// <summary>
        /// Link an input pin of an instruction to an output pin of another in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function in which retreive the instructions</param>
        /// <param name="fromID">Identifier of the instruction from which link the output</param>
        /// <param name="outputName">Name of the output to link</param>
        /// <param name="toID">Identifier of the instruction to which link to input</param>
        /// <param name="intputName">Name of the input to link</param>
        public void linkInstructionData(UInt32 functionID, UInt32 fromID, string outputName, UInt32 toID, string intputName)
        {
            CorePackage.Entity.Function func = entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.Instruction>(toID).GetInput(intputName).LinkTo(func.findInstruction<CorePackage.Execution.Instruction>(fromID), outputName);
        }

        /// <summary>
        /// Set an input value of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function from which retreive the instruction</param>
        /// <param name="instruction">Identifier of the instruction that contains the input to set</param>
        /// <param name="inputname">Name of the input to which set the value</param>
        /// <param name="inputValue">Value to set to the input</param>
        public void setInstructionInputValue(UInt32 functionID, UInt32 instruction, string inputname, dynamic inputValue)
        {
            CorePackage.Entity.Function func = entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.Instruction>(instruction).SetInputValue(inputname, inputValue);
        }

        /// <summary>
        /// Unlink an execution pin of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function that contains the instruction to unlink</param>
        /// <param name="instruction">Identifier of the instruction to unlink in the function</param>
        /// <param name="outIndex">Index of the pin to unlink in the instruction</param>
        public void unlinkInstructionFlow(UInt32 functionID, UInt32 instruction, uint outIndex)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(instruction).Unlink(outIndex);
        }

        /// <summary>
        /// Unlink an input of an instruction in a specific function
        /// </summary>
        /// <param name="functionID">Identifier of the function that contains the instruction</param>
        /// <param name="instruction">Identifier of the instruction to unlink in the function</param>
        /// <param name="inputname">Name of the input to unlink</param>
        public void unlinkInstructionInput(UInt32 functionID, UInt32 instruction, string inputname)
        {
            entity_factory.findDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.Instruction>(instruction).GetInput(inputname).Unlink();
        }
    }
}
