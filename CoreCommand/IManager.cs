using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Method to handle a CoreControl.Controller.Declare call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void onDeclare(Stream inStream, Stream outStream);

        /// <summary>
        /// Method to handle CoreControl.Controller.SetVariableValue call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void onSetVariableValue(Stream inStream, Stream outStream);

        /// <summary>
        /// Method to handle CoreControl.Controller.SetVariableType call
        /// </summary>
        /// <param name="inStream">Input stream from which read the command</param>
        /// <param name="outStream">Output stream on which write the reply</param>
        void onSetVariableType(Stream inStream, Stream outStream);
    }
}
