using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class SetVariableValue : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetVariableValue(VariableID, JsonConvert.DeserializeObject(Value));
            return null;
        }
    }
}