using CorePackage;
using System;
using System.Collections.Generic;

namespace CoreControl
{
    public class Controler
    {
        private EntityFactory entity_factory = new EntityFactory();

        //on entity removed

        //on instruction unlinked flow

        //on instruction unlinked data

        //entityFactory => can easily create and found entities
        //For a class, if you want to focus static variables, focus its internal context
        //Declaring a function into a context just create it, adding into a class add a 'this' argument to the function

        private T findDefinitionOfType<T>(UInt32 id)
        {
            return (T)Convert.ChangeType(entity_factory.find(id), typeof(T));
        }

        private CorePackage.Global.IDeclarator<T> getDeclaratorOf<T>(UInt32 id)
        {
            return findDefinitionOfType<CorePackage.Global.IDeclarator<T>>(id);
        }

        public UInt32 declare<T>(UInt32 containerID, string name, CorePackage.Global.AccessMode visibility) where T : CorePackage.Global.Definition
        {
            getDeclaratorOf<T>(containerID).Declare(entity_factory.create<T>(), name, visibility);
            return entity_factory.CurrentID - 1;
        }

        public void remove<T>(UInt32 containerID, string name) where T : CorePackage.Global.Definition
        {
            getDeclaratorOf<T>(containerID).Pop(name);
        }

        public void rename<T>(UInt32 containerID, string lastName, string newName) where T : CorePackage.Global.Definition
        {
            getDeclaratorOf<T>(containerID).Rename(lastName, newName);
        }

        public void move<T>(UInt32 fromID, UInt32 toID, string name)
        {
            CorePackage.Global.IDeclarator<T> from = getDeclaratorOf<T>(fromID);
            CorePackage.Global.IDeclarator<T> to = getDeclaratorOf<T>(toID);

            CorePackage.Global.AccessMode visibility = new CorePackage.Global.AccessMode();
            T definition = from.GetVisibilityOf(name, ref visibility);
            to.Declare(definition, name, visibility);
            from.Pop(name);
        }

        public void changeVisibility<T>(UInt32 containerID, string name, CorePackage.Global.AccessMode newVisi)
        {
            getDeclaratorOf<T>(containerID).ChangeVisibility(name, newVisi);
        }

        //declare => need a name, a visibility and an entity to which add it
        //  - Variable (can be on context, enum type, class type, function)
        //  - Context (can be add on context or global)
        //  - Type (can be add on context)
        //  - Function (can be add on context or class type)
        // returns the ID from which retreive the declaration
        //
        // get the entity from the factory with given id
        // if there is no entity
        //      if WantedEntity is Context
        //          declare into global context
        //      else
        //          throw error
        // else
        //      cast the entity into Declarator<[WantedEntity]>
        //      if cast fails
        //          throw error
        //      declare into retreived entity

        //remove => parent entity id and a declaration name

        //rename => parent entity id, last name and new name

        //move => from id, to id, declaration name

        //change visibility => entity id, declaration name, visibility

        //edit => need an entity id
        //  - Variable :
        //      - Set value : need a value
        //      - Set type :  need an entity id
        //  - Context :
        //      - Change parent : need an entity id
        //  - Type :
        //      - Scalar : nothing
        //      - Enum :
        //          - Set type : entity id
        //          - set value : name value (create/edit)
        //          - remove value : name
        //      - Class : 
        //          - add public attribute : name, type
        //          - add private attribute : name, type
        //          - rename attribute : lastname, newname
        //          - remove attribute : name
        //          - set member method : name, visibility (can add/edit)
        //      - List :
        //          - Set type : entity id
        //  - Function :
        //      - Set entry point : internal instruction index
        //      - Add instruction : instruction type id [specific for each instructions]
        //      - Remove instruction : instruction index

        public void setVariableValue(UInt32 variableID, dynamic value)
        {
            findDefinitionOfType<CorePackage.Entity.Variable>(variableID).Value = value;
        }

        public void setVariableType(UInt32 variableID, UInt32 typeID)
        {
            findDefinitionOfType<CorePackage.Entity.Variable>(variableID).Type = findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        public void setContextParent(UInt32 contextID, UInt32 parentID)
        {
            findDefinitionOfType<CorePackage.Global.IContext>(contextID).SetParent(findDefinitionOfType<CorePackage.Global.IContext>(parentID));
        }

        public void setEnumerationType(UInt32 enumID, UInt32 typeID)
        {
            findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).Stored = findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        public void setEnumerationValue(UInt32 enumID, string name, dynamic value)
        {
            findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).SetValue(name, value);
        }

        public void removeEnumerationValue(UInt32 enumID, string name)
        {
            findDefinitionOfType<CorePackage.Entity.Type.EnumType>(enumID).RemoveValue(name);
        }

        public void addClassAttribute(UInt32 classID, string name, UInt32 typeID, CorePackage.Global.AccessMode visibility)
        {
            findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).AddAttribute(name, findDefinitionOfType<CorePackage.Entity.DataType>(typeID), visibility);
        }

        public void renameClassAttribute(UInt32 classID, string lastName, string newName)
        {
            findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RenameAttribute(lastName, newName);
        }

        public void removeClassAttribute(UInt32 classID, string name)
        {
            findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID).RemoveAttribute(name);
        }

        public UInt32 addClassMemberFunction(UInt32 classID, string name, CorePackage.Global.AccessMode visibility)
        {
            UInt32 funcID = declare<CorePackage.Entity.Function>(classID, name, visibility);
            CorePackage.Entity.Type.ObjectType obj = findDefinitionOfType<CorePackage.Entity.Type.ObjectType>(classID);
            CorePackage.Entity.Function func = ((CorePackage.Global.IDeclarator<CorePackage.Entity.Function>)obj).Find(name, visibility);

            func.Declare(new CorePackage.Entity.Variable(obj), "this", CorePackage.Global.AccessMode.EXTERNAL);
            func.SetVariableAs("this", CorePackage.Entity.Function.VariableRole.PARAMETER);
            return funcID;
        }

        public void setListType(UInt32 listID, UInt32 typeID)
        {
            findDefinitionOfType<CorePackage.Entity.Type.ListType>(listID).Stored = findDefinitionOfType<CorePackage.Entity.DataType>(typeID);
        }

        public void setFunctionParameter(UInt32 funcID, string externalVarName)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.PARAMETER);
        }

        public void setFunctionReturn(UInt32 funcID, string externalVarName)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(funcID).SetVariableAs(externalVarName, CorePackage.Entity.Function.VariableRole.RETURN);
        }

        public void setFunctionEntryPoint(UInt32 functionID, UInt32 instruction)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(functionID).setEntryPoint(instruction);
        }

        public void removeFunctionInstruction(UInt32 functionID, UInt32 instruction)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(functionID).removeInstruction(instruction);
        }

        public UInt32 addInstruction(UInt32 functionID, InstructionFactory.INSTRUCTION_ID to_create, List<UInt32> crea_args)
        {
            CorePackage.Entity.Function func = findDefinitionOfType<CorePackage.Entity.Function>(functionID);
            List<CorePackage.Global.Definition> crea_definitions = new List<CorePackage.Global.Definition>();

            foreach (UInt32 definitionID in crea_args)
            {
                crea_definitions.Add(entity_factory.find(definitionID));
            }
            return func.addInstruction(InstructionFactory.create_instruction(to_create, crea_definitions));
        }

        //instruction manipulation : need function id
        //  - flow link : from id, out index, to id
        //  - data link : from id, output name, to id, input name
        //  - set input value : instruction id, intput name, input value
        //  - unlink flow : instruction, out index
        //  - unlink data : instruction, input name

        public void linkInstructionExecution(UInt32 functionID, UInt32 fromID, UInt32 outIndex, UInt32 toID)
        {
            CorePackage.Entity.Function func = findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(fromID).LinkTo(outIndex, func.findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(toID));
        }

        public void linkInstructionData(UInt32 functionID, UInt32 fromID, string outputName, UInt32 toID, string intputName)
        {
            CorePackage.Entity.Function func = findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.Instruction>(toID).GetInput(intputName).LinkTo(func.findInstruction<CorePackage.Execution.Instruction>(fromID), outputName);
        }

        public void setInstructionInputValue(UInt32 functionID, UInt32 instruction, string inputname, dynamic inputValue)
        {
            CorePackage.Entity.Function func = findDefinitionOfType<CorePackage.Entity.Function>(functionID);

            func.findInstruction<CorePackage.Execution.Instruction>(instruction).SetInputValue(inputname, inputValue);
        }

        public void unlinkInstructionFlow(UInt32 functionID, UInt32 instruction, uint outIndex)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.ExecutionRefreshInstruction>(instruction).Unlink(outIndex);
        }

        public void unlinkInstructionInput(UInt32 functionID, UInt32 instruction, string inputname)
        {
            findDefinitionOfType<CorePackage.Entity.Function>(functionID).findInstruction<CorePackage.Execution.Instruction>(instruction).GetInput("inputname").Unlink();
        }
    }
}
