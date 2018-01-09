using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EventServerClient.Communication;

namespace EventServerClientTest
{
    class Program
    {

        class TestReceive {
            public int n = 3;

            public TestReceive(int a) {
                this.n = a;
            }

            public void OnReceivePopole(byte[] data)
            {
                int lol = BitConverter.ToInt32(data, 0);
                Console.WriteLine("POPOLE" + lol);
                Console.WriteLine(this.n);
            }
        }

        static void Main(string[] args)
        {
            TcpManager client = new TcpManager();
            TestReceive test = new TestReceive(4);
            
            client.Connect("69.159.172.137", 7777);
            client.RegisterEvent("POPOLE", test.OnReceivePopole, 4);
            while (true) {
                client.Update();
                Thread.Sleep(20);
                int intValue = 8888;
                byte[] intBytes = BitConverter.GetBytes(intValue);
                client.SendEvent("POPOLE", intBytes);
            }
        }
    }
}
