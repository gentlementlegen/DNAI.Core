using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class Declare : ICommand<Declare.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public Declare Command { get; set; }

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
            Console.WriteLine("Declaring: {" + EntityType.ToString() + ", " + ContainerID.ToString() + ", " + Name + ", " + Visibility.ToString() + "}");
            return new Reply
            {
                Command = this,
                EntityID = controller.Declare(EntityType, ContainerID, Name, Visibility)
            };
        }
    }
}