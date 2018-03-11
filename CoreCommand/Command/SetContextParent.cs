using System;
using CoreControl;

namespace CoreCommand.Command
{
    public class SetContextParent : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 ContextId { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 ParentId { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetContextParent(ContextId, ParentId);
            return null;
        }
    }
}