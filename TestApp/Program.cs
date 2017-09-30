using CorePackage.Communication;
using CorePackageNet.Communication;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpManager tcp = new TcpManager(4242);

            tcp.StartListeningAsync();

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 4242);
            //s.Bind(localEndPoint);
            s.Connect(localEndPoint);

            var str = new NetworkStream(s);
            while (true)
            {
                Serializer.SerializeWithLengthPrefix<PacketRegisterEventRequest>(str, new PacketRegisterEventRequest() { Id = 3 /*ClientName = "mabit"*/ }, PrefixStyle.Base128);
            }
        }
    }
}
