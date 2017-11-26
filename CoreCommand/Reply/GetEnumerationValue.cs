using CoreCommand.Command;

namespace CoreCommand.Reply
{
    [ProtoBuf.ProtoContract]
    internal class GetEnumerationValue
    {
        [ProtoBuf.ProtoMember(1)]
        public Command.GetEnumerationValue Command { get; set; }
        [ProtoBuf.ProtoMember(2)]
        public dynamic Value { get; set; }
    }
}