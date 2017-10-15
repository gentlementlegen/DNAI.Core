using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Communication;
using System.Net.Sockets;
using System.Net;
using CorePackageNet.Communication;
using ProtoBuf;

namespace TestCore
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    TcpManager tcp = new TcpManager(4242);

        //    tcp.StartListeningAsync();

        //    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //    IPAddress ipAddress = ipHostInfo.AddressList[0];
        //    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 4242);
        //    //s.Bind(localEndPoint);
        //    s.Connect(localEndPoint);

        //    var str = new NetworkStream(s);
        //    Serializer.SerializeWithLengthPrefix<PacketRegisterEventRequest>(str, new PacketRegisterEventRequest() { Id = 3 }, PrefixStyle.Base128);
        //}
    }
}
