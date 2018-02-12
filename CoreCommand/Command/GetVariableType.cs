using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command
{
    public class GetVariableType : ICommand<GetVariableType.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Command = this,
                TypeID = controller.GetVariableType(VariableID)
            };
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public GetVariableType Command { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt32 TypeID { get; set; }
        }
    }
}
