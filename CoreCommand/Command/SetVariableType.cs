using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command
{
    public class SetVariableType : ICommand<SetVariableType.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetVariableType Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 TypeID { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetVariableType(VariableID, TypeID);
            return new Reply
            {
                Command = this
            };
        }
    }
}
