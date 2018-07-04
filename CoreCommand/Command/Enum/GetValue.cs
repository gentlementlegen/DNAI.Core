using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command.Enum
{
    public class GetValue : ICommand<GetValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public string Value { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public string Name { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint EnumId { get; set; }

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                Value = JsonConvert.SerializeObject(controller.GetEnumerationValue(EnumId, Name))
            };
        }
    }
}