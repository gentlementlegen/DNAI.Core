using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class AddClassMemberFunction : ICommand<AddClassMemberFunction.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public AddClassMemberFunction Command { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt32 MethodID { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public EntityFactory.VISIBILITY Visibility { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Command = this,
                MethodID = controller.AddClassMemberFunction(ClassId, Name, Visibility)
            };
        }
    }
}