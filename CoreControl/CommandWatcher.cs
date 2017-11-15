using System;
using System.Collections.Generic;
using System.IO;

namespace CoreControl
{
    public class CommandWatcher
    {
        private readonly List<Func<object>> _funcList = new List<Func<object>>();

        public void AddCommand(Func<object> function)
        {
            _funcList.Add(function);
        }

        public void SerializeCommandsToFile(string path = "dulyIA.duly")
        {
            var stream = File.Create(path);

            foreach (var item in _funcList)
            {
                ProtoBuf.Serializer.SerializeWithLengthPrefix<Command.BaseAction>(stream, item.Invoke() as Command.BaseAction, ProtoBuf.PrefixStyle.Base128);
            }

            stream.Dispose();

            Console.WriteLine("Wrote Duly file to path => " + stream.Name);
        }

        public List<Command.BaseAction> DeserializeCommandsFromFile(string path = "dulyIA.duly")
        {
            List<Command.BaseAction> actions = new List<Command.BaseAction>();
            var stream = File.OpenRead(path);

            Command.BaseAction b;
            while ((b = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Command.BaseAction>(stream, ProtoBuf.PrefixStyle.Base128)) != null)
            {
                actions.Add(b);
            }

            stream.Dispose();
            return actions;
        }
    }
}