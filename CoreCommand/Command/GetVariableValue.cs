using System;
using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class GetVariableValue : ICommand<GetVariableValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public string Value { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 VariableId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Value = JsonConvert.SerializeObject(controller.GetVariableValue(VariableId))
            };
        }
    }
}