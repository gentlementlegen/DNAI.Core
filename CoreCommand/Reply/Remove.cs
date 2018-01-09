using System.Collections.Generic;

namespace CoreCommand.Reply
{
    public class Remove
    {
        [BinarySerializer.BinaryFormat]
        public Command.Remove Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<uint> Removed { get; set; }
    }
}