using System;

namespace CorePluginLego.Model
{
    public interface IConnection
    {
        bool IsConnected { get; }
    }

    public interface IConnection<T> : IConnection, IDisposable
    {
        System.Threading.Tasks.Task<T> Connect();
    }
}