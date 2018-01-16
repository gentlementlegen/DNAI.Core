using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreDaemon
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreNetwork.ClientManager client = new CoreNetwork.ClientManager(new CoreCommand.ProtobufManager());

            //client.Connect("10.248.84.63", 7777);
            int port = 7777;
            Console.WriteLine(args.Count());
            if (args.Count() == 2 && args[0] == "-p") {
                port = Int32.Parse(args[1]);
            }
            Console.WriteLine(port);
            client.Connect("127.0.0.1", port);

            if (!client.isConnected())
                throw new Exception("Unable to connect");

            client.RegisterEvents();

            while (client.isConnected())
            {
                client.Update();
                Thread.Sleep(20);
            }
        }
    }
}
