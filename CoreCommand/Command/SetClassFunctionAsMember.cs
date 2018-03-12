using CoreControl;
using System;

namespace CoreCommand.Command
{
    public class SetClassFunctionAsMember : ICommand<SetClassFunctionAsMember.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetClassFunctionAsMember Command { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt32 ThisParamID { get; set; }
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
                ThisParamID = controller.SetClassFunctionAsMember(ClassId, Name, Visibility)
            };
        }
    }
}