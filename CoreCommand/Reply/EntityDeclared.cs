using System;

namespace CoreCommand.Reply
{
    public class EntityDeclared
    {
        [BinarySerializer.BinaryFormat]
        public Command.Declare Command { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 EntityID { get; set; }
    }
}