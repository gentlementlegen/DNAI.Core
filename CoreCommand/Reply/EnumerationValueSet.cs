using CoreCommand.Command;

namespace CoreCommand.Reply
{
    public class EnumerationValueSet
    {
        [BinarySerializer.BinaryFormat]
        public Command.SetEnumerationValue Command { get; set; }
    }
}