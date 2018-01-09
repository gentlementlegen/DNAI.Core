using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class Remove
    {
        [BinarySerializer.BinaryFormat]
        public EntityFactory.ENTITY EntityType { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }
    }
}