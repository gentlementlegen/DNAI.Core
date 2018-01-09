using System;

namespace CoreCommand.Command
{
    public class SetContextParent
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 ContextId { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ParentId { get; set; }
    }
}