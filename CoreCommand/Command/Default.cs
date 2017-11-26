using static CoreControl.InstructionFactory;

namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class Default
    {
        [ProtoBuf.ProtoMember(1)]
        public Declare Declare { get { return new Declare(); } }

        [ProtoBuf.ProtoMember(2)]
        public Remove Remove { get { return new Remove(); } }

        [ProtoBuf.ProtoMember(3)]
        public Rename Rename { get { return new Rename(); } }

        [ProtoBuf.ProtoMember(4)]
        public Move Move { get { return new Move(); } }

        [ProtoBuf.ProtoMember(5)]
        public ChangeVisibility ChangeVisibility { get { return new ChangeVisibility(); } }
        
        [ProtoBuf.ProtoMember(6)]
        public SetVariableValue SetVariableValue { get { return new SetVariableValue(); } }

        [ProtoBuf.ProtoMember(7)]
        public SetVariableType SetVariableType { get { return new SetVariableType(); } }

        [ProtoBuf.ProtoMember(8)]
        public GetVariableValue GetVariableValue { get { return new GetVariableValue(); } }

        [ProtoBuf.ProtoMember(9)]
        public SetContextParent SetContextParent { get { return new SetContextParent(); } }

        [ProtoBuf.ProtoMember(10)]
        public SetEnumerationType SetEnumerationType { get { return new SetEnumerationType(); } }

        [ProtoBuf.ProtoMember(11)]
        public SetEnumerationValue SetEnumerationValue { get { return new SetEnumerationValue(); } }

        [ProtoBuf.ProtoMember(12)]
        public GetEnumerationValue GetEnumerationValue { get { return new GetEnumerationValue(); } }

        [ProtoBuf.ProtoMember(13)]
        public RemoveEnumerationValue RemoveEnumerationValue { get { return new RemoveEnumerationValue(); } }

        [ProtoBuf.ProtoMember(14)]
        public AddClassAttribute AddClassAttribute { get { return new AddClassAttribute(); } }

        [ProtoBuf.ProtoMember(15)]
        public RenameClassAttribute RenameClassAttribute { get { return new RenameClassAttribute(); } }

        [ProtoBuf.ProtoMember(16)]
        public RemoveClassAttribute RemoveClassAttribute { get { return new RemoveClassAttribute(); } }

        [ProtoBuf.ProtoMember(17)]
        public AddClassMemberFunction AddClassMemberFunction { get { return new AddClassMemberFunction(); } }

        [ProtoBuf.ProtoMember(18)]
        public SetListType SetListType { get { return new SetListType(); } }

        [ProtoBuf.ProtoMember(19)]
        public CallFunction CallFunction { get { return new CallFunction(); } }

        [ProtoBuf.ProtoMember(20)]
        public SetFunctionParameter SetFunctionParameter { get { return new SetFunctionParameter(); } }

        [ProtoBuf.ProtoMember(21)]
        public SetFunctionReturn SetFunctionReturn { get { return new SetFunctionReturn(); } }

        [ProtoBuf.ProtoMember(22)]
        public SetFunctionEntryPoint SetFunctionEntryPoint { get { return new SetFunctionEntryPoint(); } }

        [ProtoBuf.ProtoMember(23)]
        public RemoveFunctionInstruction RemoveFunctionInstruction { get { return new RemoveFunctionInstruction(); } }

        [ProtoBuf.ProtoMember(24)]
        public AddInstruction AddInstruction { get { return new AddInstruction(); } }

        [ProtoBuf.ProtoMember(25)]
        public LinkInstructionExecution LinkInstructionExecution { get { return new LinkInstructionExecution(); } }

        [ProtoBuf.ProtoMember(26)]
        public LinkInstructionData LinkInstructionData { get { return new LinkInstructionData(); } }

        [ProtoBuf.ProtoMember(27)]
        public SetInstructionInputValue SetInstructionInputValue { get { return new SetInstructionInputValue(); } }

        [ProtoBuf.ProtoMember(28)]
        public UnlinkInstructionFlow UnlinkInstructionFlow { get { return new UnlinkInstructionFlow(); } }

        [ProtoBuf.ProtoMember(29)]
        public UnlinkInstructionInput UnlinkInstructionInput { get { return new UnlinkInstructionInput(); } }
    }
}