using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class GetEnumerationValue : ICommand<GetEnumerationValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public GetEnumerationValue Command { get; set; }

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
                Command = this,
                Value = JsonConvert.SerializeObject(controller.GetEnumerationValue(EnumId, Name))
            };
        }
    }
}