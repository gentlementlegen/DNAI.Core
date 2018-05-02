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

        /// <summary>
        /// Calls a command registered in the Binary manager for resolution
        /// </summary>
        /// <param name="command">Name of the command to call</param>
        /// <param name="inStream">Input stream from which deserialize the Command</param>
        /// <param name="outStream">Output stream to where serialize reply</param>
        /// <returns>True if command have been correctly processed, false either</returns>
        bool CallCommand(string command, Stream inStream, Stream outStream);
        
        /// <summary>
        /// Get the dictionnary of the command/reply name association
        /// </summary>
        /// <returns>Dictionnary that associates command name to reply name</returns>
        Dictionary<String, String> GetRegisteredCommands();

        /// <summary>
        /// Get the name of a specific command from its type
        /// </summary>
        /// <param name="commandType">Type of the command from which retreive the name</param>
        /// <returns>Name of the command corresponding to the type</returns>
        String GetCommandName(Type commandType);

        /// <summary>
        /// Clear the entire controller and saved command list
        /// </summary>
        void Reset();
    }
}