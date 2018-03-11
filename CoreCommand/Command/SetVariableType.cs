using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command
{
    public class SetVariableType : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public UInt32 TypeID { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetVariableType(VariableID, TypeID);
            return null;
        }
    }
}
