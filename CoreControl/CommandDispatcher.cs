using System;
using System.IO;

namespace CoreControl
{
    /// <summary>
    /// Event representing a command change.
    /// </summary>
    public class CommandEvent : EventArgs
    {
        /// <summary>
        /// The type of event (added, removed)
        /// </summary>
        public enum EventType { ADD = 0, REMOVE }

        public EventType Type { get; }

        public CommandEvent(EventType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// Dispatcher that handles the command events, updating the watcher accordingly.
    /// </summary>
    public class CommandDispatcher
    {
        /// <summary>
        /// Event called when a command changed, added or removed.
        /// </summary>
        public event EventHandler<CommandEvent> OnCommandChanged;

        private readonly Controller _controller = new Controller();
        private readonly CommandWatcher _watcher = new CommandWatcher();
        private readonly Stream _inStream;
        private readonly Stream _outStream;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="stream">The stream where to read / write.</param>
        public CommandDispatcher(Stream stream)
        {
            _inStream = stream;
            _outStream = stream;
        }
    }
}