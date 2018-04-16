using System;
using System.Collections.Generic;
using System.IO;

namespace CoreCommand
{
    public enum COMMANDS
    {
        DECLARE
    }

    public interface IManager
    {
        /// <summary>
        /// Serialize saved commands into a file
        /// </summary>
        /// <param name="filename">Name of the file in which serialize commands</param>
        void SaveCommandsTo(string filename);

        /// <summary>
        /// Deserialize saved commands from a file
        /// </summary>
        /// <param name="filename">Name of the file from which load commands</param>
        void LoadCommandsFrom(string filename);

        bool CallCommand(string command, Stream inStream, Stream outStream);

        Func<Stream, Stream, bool> GetCommand(String name);

        String GetCommandName(Type commandType);

        Dictionary<String, String> GetRegisteredCommands();

        void Reset();
    }
}