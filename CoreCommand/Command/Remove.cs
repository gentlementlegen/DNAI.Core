using CoreControl;
using System;
using System.Collections.Generic;

namespace CoreCommand.Command
{
    public class Remove : ICommand<Remove.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public List<UInt32> Removed { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.ENTITY EntityType { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Removed = controller.Remove(EntityType, ContainerID, Name)
            };
        }
    }
}