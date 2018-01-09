using CoreCommand.Command;

namespace CoreCommand.Reply
{
    internal class EnumerationValueGet
    {
        [BinarySerializer.BinaryFormat]
        public Command.GetEnumerationValue Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }
    }
}