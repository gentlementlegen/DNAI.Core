using ProtoBuf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EventServerClient.Communication
{
    public class TcpManager
    {
        private readonly TcpClient _tcpClient;
        private readonly CreatePackage _createPackage;
        private byte[] _dataStorage;
        private ValidatePackage _validatePackage;
        private System.Collections.Generic.Dictionary<string, Action<byte[]>> _mapPtr;


        public TcpManager()
        {
            _tcpClient = new TcpClient();
            _createPackage = new CreatePackage();
            _dataStorage = null;
            _validatePackage = new ValidatePackage();
            _mapPtr = new System.Collections.Generic.Dictionary<string, Action<byte[]>>();
        }

        public void Connect(string address, int port)
        {
            try
            {

                _tcpClient.Connect(address, port);
              
                Byte[] data = _createPackage.AuthenticatePackage("YOLO fernand");
                _tcpClient.GetStream().Write(data, 0, data.Length);
                
            }
            catch
            {
                Console.Write("failed");
            }
        }

        public Task ConnectAsync(string address, int port)
        {
            return Task.Run(() =>
            {
                Connect(address, port);
            });
        }

        public void Disconnect()
        {
            _tcpClient.Close();
        }

        public void RegisterEvent(string eventName, Action<byte[]> func, uint size) {
            Byte[] dataRegisterEvent = _createPackage.EventRegisterPackage(eventName, size, true);
            _tcpClient.GetStream().Write(dataRegisterEvent, 0, dataRegisterEvent.Length);
            _mapPtr.Add(eventName, func);
        }

        public void SendEvent(string eventName, byte[] data) {
            byte[] dataSend = _createPackage.EventSendPackage(eventName, data);
            _tcpClient.GetStream().Write(dataSend, 0, dataSend.Length);
        }

        private byte[] ReadData() {
            // Server Reply
            var networkStream = _tcpClient.GetStream();
            byte[] readBuffer = null;

            if (networkStream.CanRead)
            {
                // Buffer to store the response bytes.
                readBuffer = new byte[_tcpClient.ReceiveBufferSize];
               // string fullServerReply = null;
                using (var writer = new System.IO.MemoryStream())
                {
                    while (networkStream.DataAvailable)
                    {
                        int numberOfBytesRead = networkStream.Read(readBuffer, 0, readBuffer.Length);
                        if (numberOfBytesRead <= 0)
                        {
                            break;
                        }
                        writer.Write(readBuffer, 0, numberOfBytesRead);
                    }
                  //  fullServerReply = System.Text.Encoding.UTF8.GetString(writer.ToArray());
                }
            }
            return readBuffer;
        }

        private byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private void DataCompute(byte[] data) {
            if (_dataStorage != null) {
                _dataStorage = Combine(_dataStorage, data);
            } else {
                _dataStorage = data;
            }


            while (_dataStorage.Length >= 12)
            {
                ValidatePackage.Header head = _validatePackage.GetHeader(_dataStorage);
                if (head != null)
                {
                    if (head.id == 3)
                    {
                        ValidatePackage.ReceiveEvent receiveEvent = _validatePackage.GetReceivePackage(head, _dataStorage);
                        if (receiveEvent != null)
                        {
                            if (_mapPtr.ContainsKey(receiveEvent.eventName))
                            {
                                _mapPtr[receiveEvent.eventName](receiveEvent.data);
                            }
                            byte[] newArray = new byte[_dataStorage.Length - (12 + head.size)];
                            Buffer.BlockCopy(_dataStorage, (int)(12 + head.size), newArray, 0, newArray.Length);
                            _dataStorage = newArray;
                        }
                    } else {
                        _dataStorage = null;
                    }
                }
                else if (_dataStorage.Length >= 12)
                {
                    _dataStorage = null;
                    break;
                } else {
                    break;
                }
            }
        }

        public void Update() {
            byte[] data = ReadData();
            if (data != null)
            {
                DataCompute(data);
            }
        }

    }
}