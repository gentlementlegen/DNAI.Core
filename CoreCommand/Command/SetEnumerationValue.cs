using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class SetEnumerationValue : ICommand<SetEnumerationValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetEnumerationValue Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public string Value { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetEnumerationValue(EnumId, Name, JsonConvert.DeserializeObject(Value));
            return new Reply
            {
                Command = this
            };
        }
    }
}