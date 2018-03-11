using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class Declare : ICommand<Declare.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UInt32 EntityID { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.ENTITY EntityType { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContainerID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                EntityID = controller.Declare(EntityType, ContainerID, Name, Visibility)
            };
        }
    }
}