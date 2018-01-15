using System;
using CoreControl;

namespace CoreCommand.Command
{
    public class SetContextParent : ICommand<SetContextParent.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetContextParent Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 ContextId { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ParentId { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetContextParent(ContextId, ParentId);
            return new Reply
            {
                Command = this
            };
        }
    }
}