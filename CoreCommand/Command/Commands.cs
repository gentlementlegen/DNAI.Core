using static CoreControl.InstructionFactory;

namespace CoreCommand.Command
{
    public class Commands
    {
        [BinarySerializer.BinaryFormat]
        public Declare Declare { get { return new Declare(); } }

        [BinarySerializer.BinaryFormat]
        public Remove Remove { get { return new Remove(); } }

        [BinarySerializer.BinaryFormat]
        public Rename Rename { get { return new Rename(); } }

        [BinarySerializer.BinaryFormat]
        public Move Move { get { return new Move(); } }

        [BinarySerializer.BinaryFormat]
        public ChangeVisibility ChangeVisibility { get { return new ChangeVisibility(); } }

        [BinarySerializer.BinaryFormat]
        public SetVariableValue SetVariableValue { get { return new SetVariableValue(); } }

        [BinarySerializer.BinaryFormat]
        public SetVariableType SetVariableType { get { return new SetVariableType(); } }

        [BinarySerializer.BinaryFormat]
        public GetVariableValue GetVariableValue { get { return new GetVariableValue(); } }

        [BinarySerializer.BinaryFormat]
        public SetContextParent SetContextParent { get { return new SetContextParent(); } }

        [BinarySerializer.BinaryFormat]
        public SetEnumerationType SetEnumerationType { get { return new SetEnumerationType(); } }

        [BinarySerializer.BinaryFormat]
        public SetEnumerationValue SetEnumerationValue { get { return new SetEnumerationValue(); } }

        [BinarySerializer.BinaryFormat]
        public GetEnumerationValue GetEnumerationValue { get { return new GetEnumerationValue(); } }

        [BinarySerializer.BinaryFormat]
        public RemoveEnumerationValue RemoveEnumerationValue { get { return new RemoveEnumerationValue(); } }

        [BinarySerializer.BinaryFormat]
        public AddClassAttribute AddClassAttribute { get { return new AddClassAttribute(); } }

        [BinarySerializer.BinaryFormat]
        public RenameClassAttribute RenameClassAttribute { get { return new RenameClassAttribute(); } }

        [BinarySerializer.BinaryFormat]
        public RemoveClassAttribute RemoveClassAttribute { get { return new RemoveClassAttribute(); } }

        [BinarySerializer.BinaryFormat]
        public SetClassFunctionAsMember AddClassMemberFunction { get { return new SetClassFunctionAsMember(); } }

        [BinarySerializer.BinaryFormat]
        public SetListType SetListType { get { return new SetListType(); } }

        [BinarySerializer.BinaryFormat]
        public CallFunction CallFunction { get { return new CallFunction(); } }

        [BinarySerializer.BinaryFormat]
        public SetFunctionParameter SetFunctionParameter { get { return new SetFunctionParameter(); } }

        [BinarySerializer.BinaryFormat]
        public SetFunctionReturn SetFunctionReturn { get { return new SetFunctionReturn(); } }

        [BinarySerializer.BinaryFormat]
        public SetFunctionEntryPoint SetFunctionEntryPoint { get { return new SetFunctionEntryPoint(); } }

        [BinarySerializer.BinaryFormat]
        public RemoveFunctionInstruction RemoveFunctionInstruction { get { return new RemoveFunctionInstruction(); } }

        [BinarySerializer.BinaryFormat]
        public AddInstruction AddInstruction { get { return new AddInstruction(); } }

        [BinarySerializer.BinaryFormat]
        public LinkInstructionExecution LinkInstructionExecution { get { return new LinkInstructionExecution(); } }

        [BinarySerializer.BinaryFormat]
        public LinkInstructionData LinkInstructionData { get { return new LinkInstructionData(); } }

        [BinarySerializer.BinaryFormat]
        public SetInstructionInputValue SetInstructionInputValue { get { return new SetInstructionInputValue(); } }

        [BinarySerializer.BinaryFormat]
        public UnlinkInstructionFlow UnlinkInstructionFlow { get { return new UnlinkInstructionFlow(); } }

        [BinarySerializer.BinaryFormat]
        public UnlinkInstructionInput UnlinkInstructionInput { get { return new UnlinkInstructionInput(); } }
    }
}