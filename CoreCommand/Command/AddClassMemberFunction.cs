using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class AddClassMemberFunction
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }
    }
}