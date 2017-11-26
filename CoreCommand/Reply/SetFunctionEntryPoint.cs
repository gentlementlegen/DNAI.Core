namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetFunctionEntryPoint
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetFunctionEntryPoint Command { get; set; }
    }
}