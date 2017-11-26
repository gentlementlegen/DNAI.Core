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

            client.Connect("10.248.18.70", 7777);
            //client.Connect("127.0.0.1", 8765);

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
