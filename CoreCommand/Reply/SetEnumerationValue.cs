using CoreCommand.Command;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class SetEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetEnumerationValue Command { get; set; }
    }
}