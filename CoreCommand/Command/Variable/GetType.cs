using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Variable
{
    public class GetType : ICommand<GetType.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                TypeID = controller.GetVariableType(VariableID)
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UInt32 TypeID { get; set; }
        }
    }
}
