using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreControl;

namespace CoreCommand.Command.Enum
{
    public class GetValues : ICommand<GetValues.Reply>
    {
        [BinarySerializer.BinaryFormat]
        public uint EntityId { get; set; }

        public Reply Resolve(Controller controller)
        {
            var values = controller.GetFullEnumerationValues(EntityId);
            var reply = new Reply { Values = new Dictionary<string, string>() };

            foreach (var pair in values)
            {
                reply.Values[pair.Key] = Newtonsoft.Json.JsonConvert.SerializeObject(pair.Value);
            }

            return reply;
        }

        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public Dictionary<string, string> Values { get; set; }
        }
    }
}
