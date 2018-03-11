using System.Collections.Generic;
using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class CallFunction : ICommand<CallFunction.Reply>
    {
        public class Reply
        {            
            [BinarySerializer.BinaryFormat]
            public Dictionary<string, string> Returns { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FuncId { get; set; }

        [BinarySerializer.BinaryFormat]
        public Dictionary<string, string> Parameters { get; set; }

        private Dictionary<string, dynamic> GetParams()
        {
            Dictionary<string, dynamic> toret = new Dictionary<string, dynamic>();

            foreach (KeyValuePair<string, string> item in Parameters)
            {
                toret[item.Key] = JsonConvert.DeserializeObject(item.Value);
            }
            return toret;
        }

        public Reply Resolve(Controller controller)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            foreach (KeyValuePair<string, dynamic> item in controller.CallFunction(FuncId, GetParams()))
            {
                res[item.Key] = JsonConvert.SerializeObject(item.Value);
            }
            return new Reply
            {
                Returns = res
            };
        }
    }
}