using CorePackageNet.Communication;
using ProtoBuf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CorePackage.Communication
{
    public class TcpManager
    {
        private readonly TcpListener _tcpListener;
        public static ManualResetEvent _tcpClientConnected = new ManualResetEvent(false);

        public TcpManager(int port)
        {
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            _tcpListener = new TcpListener(localEndPoint);
        }

        public void StartListening()
        {
            try
            {
                _tcpListener.Start();
                _tcpClientConnected.Reset();

                _tcpListener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), _tcpListener);

                // Wait until a connection is made and processed before
                // continuing.
                _tcpClientConnected.WaitOne();
            }
            catch
            {
            }
        }

        public Task StartListeningAsync()
        {
            return Task.Run(() =>
            {
                StartListening();
            });
        }

        public void StopListening()
        {
            _tcpListener.Stop();
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
            ReadData(client);

            // Signal the calling thread to continue.
            _tcpClientConnected.Set();
        }

        private void ReadData(TcpClient client)
        {
            byte[] buffer = new byte[8192];
            var stream = client.GetStream();
            PacketBase t;
            while ((t = Serializer.DeserializeWithLengthPrefix<PacketBase>(stream, PrefixStyle.Base128)) != null)
            {
                switch (t.Id)
                {
                    case 1:
                        Console.WriteLine("1.");
                        break;

                    case 2:
                        Console.WriteLine("2.");
                        break;

                    case 3:
                        Console.WriteLine("3.");
                        var x = Serializer.DeserializeWithLengthPrefix<PacketRegisterEventRequest>(stream, PrefixStyle.Base128);
                        break;
                }
            }
        }
    }
}