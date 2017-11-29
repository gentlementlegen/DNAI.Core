using CoreCommand.Command;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    public class EnumerationValueSet
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.SetEnumerationValue Command { get; set; }
    }
}