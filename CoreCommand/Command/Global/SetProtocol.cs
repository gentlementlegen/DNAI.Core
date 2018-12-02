using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Global
{
    public class SetProtocol : ICommand<EmptyReply>
    {
        public enum ProtocolValues
        {
            BINARY,
            JSON
        }

        [BinarySerializer.BinaryFormat]
        public ProtocolValues Protocol { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            return null;
        }
    }
}
