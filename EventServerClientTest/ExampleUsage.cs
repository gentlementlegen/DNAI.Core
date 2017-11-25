using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;

namespace TestCore
{
    //[TestClass]
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

       /* [TestMethod]
        public void TestClient()
        {
            TcpManager tcp = new TcpManager();
            TcpListener server = new TcpListener(4242);
            server.Start();

            server.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), server);
            tcp.Connect("127.0.0.1", 4242);
            tcp.Disconnect();
            server.Stop();
        }

        private void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on
            // the console.
            TcpClient client = listener.EndAcceptTcpClient(ar);

            // Process the connection here. (Add the client to a
            // server table, read data, etc.)
            Console.WriteLine("Client connected completed");

            var s = client.GetStream();

            Serializer.SerializeWithLengthPrefix<PacketRegisterEventRequest>(s, new PacketRegisterEventRequest { Id = 3 }, PrefixStyle.Base128);

            s.Close();

            // Signal the calling thread to continue.
        }*/
    }
}
