using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using UnityEditor;

namespace Core.Plugin.Unity.Context
{
    [InitializeOnLoad]
    public sealed class UnitySynchronizationContext : SynchronizationContext
    {
        private static readonly ConcurrentQueue<Message> Queue;

        static UnitySynchronizationContext()
        {
            Queue = new ConcurrentQueue<Message>();
            EditorApplication.update += Update;
        }

        private static void Enqueue(SendOrPostCallback d, object state)
        {
            Queue.Enqueue(new Message(d, state));
        }

        private static void Update()
        {
            if (!Queue.Any())
                return;

            Message message;

            if (!Queue.TryDequeue(out message))
                return;

            message.Callback(message.State);
        }

        public override SynchronizationContext CreateCopy()
        {
            return new UnitySynchronizationContext();
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            Enqueue(d, state);
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            Enqueue(d, state);
        }

        private sealed class Message
        {
            public Message(SendOrPostCallback callback, object state)
            {
                Callback = callback;
                State = state;
            }

            public SendOrPostCallback Callback { get; set; }
            public object State { get; set; }
        }
    }
}