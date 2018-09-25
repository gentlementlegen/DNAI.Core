using Lego.Ev3.Core;
using System;

namespace CorePluginLego.Model
{
    public class BrickController : IDisposable
    {
        private readonly IConnection _connection;
        private Brick _brick;

        public BrickController(IConnection connection)
        {
            _connection = connection;
        }

        public async System.Threading.Tasks.Task ConnectAsync()
        {
            _brick = await _connection.Connect();
            _brick.BrickChanged += Brick_BrickChanged;
        }

        private void Brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Console.WriteLine("brick changed");
        }

        public void SendCommand(Action<Brick> action)
        {
            action?.Invoke(_brick);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}