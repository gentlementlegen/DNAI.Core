using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class SetVariableValue : ICommand<SetVariableValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetVariableValue Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 VariableID { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetVariableValue(VariableID, JsonConvert.DeserializeObject(Value));
            return new Reply
            {
                Command = this
            };
        }
    }
}