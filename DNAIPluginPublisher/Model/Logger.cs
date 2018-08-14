using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace DNAIPluginPublisher.Model
{
    public enum LEVEL { Info, Warning, Error }

    /// <summary>
    /// Represents a log.
    /// </summary>
    public class LogItem
    {
        public string Message { get; }

        public DateTime Time { get; }

        public LEVEL Level { get; }

        public string MessageFormatted { get; }

        public LogItem(string message) : this(message, LEVEL.Info)
        {
        }

        public LogItem(string message, LEVEL level)
        {
            Message = message;
            Level = level;
            Time = DateTime.Now;
            MessageFormatted = $"[{Time}] {Message}";
        }
    }

    /// <summary>
    /// Class that helps for logging messages into the console.
    /// </summary>
    public static class Logger
    {
        private static readonly ObservableCollection<LogItem> _logs = new ObservableCollection<LogItem>();
        private static object _lock = new object();

        public static IReadOnlyList<LogItem> Logs => _logs;

        static Logger()
        {
            BindingOperations.EnableCollectionSynchronization(_logs, _lock);
        }

        public static void Log(string message)
        {
            _logs.Add(new LogItem(message));
        }

        public static void Log(string message, LEVEL level)
        {
            _logs.Add(new LogItem(message, level));
        }

        public static void Clear()
        {
            _logs.Clear();
        }
    }
}