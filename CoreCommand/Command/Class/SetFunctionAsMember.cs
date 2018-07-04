using CoreControl;
using System;

namespace CoreCommand.Command.Class
{
    public class SetFunctionAsMember : ICommand<SetFunctionAsMember.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UInt32 ThisParamID { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 ClassId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }
        
        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                ThisParamID = controller.SetClassFunctionAsMember(ClassId, Name)
            };
        }
    }
}