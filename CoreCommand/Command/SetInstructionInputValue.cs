namespace CoreCommand.Command
{
    [ProtoBuf.ProtoContract]
    public class SetInstructionInputValue
    {
        [ProtoBuf.ProtoMember(1)]
        public uint FunctionID { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public uint Instruction { get; set; }
        [ProtoBuf.ProtoMember(3)]
        public string InputName { get; set; }
        [ProtoBuf.ProtoMember(4)]
        public dynamic InputValue { get; set; }
    }
}