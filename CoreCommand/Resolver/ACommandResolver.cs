using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommand.Resolver
{
    public abstract class ACommandResolver
    {
        private Dictionary<string, Func<Stream, Stream, bool>> _commands = new Dictionary<string, Func<Stream, Stream, bool>>();

        /// <summary>
        /// Get the key used for a command registration from version and command name
        /// </summary>
        /// <param name="version">Version of the command</param>
        /// <param name="command">Name of the command</param>
        /// <returns>Key of the command to register</returns>
        private string GetCommandKey(string version, string command)
        {
            return version + command;
        }

        /// <summary>
        /// Register a command in the resolver with a specific version
        /// </summary>
        /// <param name="version">Version of the command</param>
        /// <param name="command">Name of the command to register</param>
        /// <param name="callback">Function to call in order to resolve command</param>
        public void Register(string version, string command, Func<Stream, Stream, bool> callback)
        {
            _commands[GetCommandKey(version, command)] = callback;
        }

        /// <summary>
        /// Resolve a command from its name and version
        /// </summary>
        /// <param name="version">Version of the command</param>
        /// <param name="command">Name of the command</param>
        /// <param name="input">Input stream to pass at resolution</param>
        /// <param name="output">Output stream to pass at resolution</param>
        /// <returns>False if resolution failed, true either</returns>
        protected bool Resolve(string version, string command, Stream input, Stream output)
        {
            if (!_commands.ContainsKey(GetCommandKey(version, command)))
                throw new KeyNotFoundException("No such package named " + command + " for version " + version);
            return _commands[GetCommandKey(version, command)](input, output);
        }

        /// <summary>
        /// Resolve a command only from it name
        /// Abstract to let children pass version name
        /// </summary>
        /// <param name="command">Name of the command to resolve</param>
        /// <param name="input">Input stream to pass at resolution</param>
        /// <param name="output">Output stream to pass at resolution</param>
        /// <returns>False if resolution failed, true either</returns>
        public abstract bool Resolve(string command, Stream input, Stream output);
    }
}
