using NKH.MindSqualls;
using System.Threading.Tasks;

namespace CorePluginLego.Model
{
    public class ConnectionBluetoothNxt : IConnection<NxtBrick>
    {
        private readonly NxtBrick _brick;

        public bool IsConnected { get; private set; }

        public ConnectionBluetoothNxt(byte port = 6)
        {
            _brick = new NxtBrick(NxtCommLinkType.Bluetooth, port);
        }

        public async Task<NxtBrick> Connect()
        {
            await Task.Run(() =>
            {
                _brick.Connect();
                IsConnected = true;
            });
            return _brick;
        }

        virtual public void Dispose()
        {
            _brick.Disconnect();
        }
    }
}