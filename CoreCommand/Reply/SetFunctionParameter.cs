namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionParameter
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetFunctionParameter Command { get; set; }
    }
}