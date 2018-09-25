using Lego.Ev3.Core;
using Lego.Ev3.Desktop;
using System;
using System.Threading.Tasks;

namespace CorePluginLego.Model
{
    public class ConnectionBluetooth : IConnection, IDisposable
    {
        private readonly Brick _brick;

        public bool IsConnected { get; private set; }

        public ConnectionBluetooth(string port = "COM3")
        {
            _brick = new Brick(new BluetoothCommunication(port));
        }

        public async Task<Brick> Connect()
        {
            await _brick.ConnectAsync();
            IsConnected = true;
            return _brick;
        }

        virtual public void Dispose()
        {
            _brick.Disconnect();
        }
    }
}