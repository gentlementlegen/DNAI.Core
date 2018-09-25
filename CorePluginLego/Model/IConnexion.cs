using System;

namespace CorePluginLego.Model
{
    public interface IConnection : IDisposable
    {
        System.Threading.Tasks.Task<Lego.Ev3.Core.Brick> Connect();

        bool IsConnected { get; }
    }
}