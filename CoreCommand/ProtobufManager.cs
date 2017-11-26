using CoreControl;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoreCommand
{
    /// <summary>
    /// Dispatcher that handles the command events, updating the watcher accordingly.
    /// </summary>
    public class ProtobufManager : IManager
    {        
        /// <summary>
        /// Controller on which dispatch command
        /// </summary>
        private readonly Controller _controller = new Controller();

        /// <summary>
        /// Internal history of dispatched commands => for serialisation
        /// </summary>
        private readonly List<dynamic> _commands = new List<dynamic>();

        /// <summary>
        /// Protobuf serialisation prefix
        /// </summary>
        private ProtoBuf.PrefixStyle _prefix = ProtoBuf.PrefixStyle.Base128;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtobufManager()
        {

        }
        
        /// <summary>
        /// Constructor to customize protobuf prefix
        /// </summary>
        /// <param name="prefix">Serialisation prefix for protobuf</param>
        public ProtobufManager(ProtoBuf.PrefixStyle prefix)
        {
            _prefix = prefix;
        }

        /// <summary>
        /// Convert a JSON value to a dynamic object
        /// </summary>
        /// <param name="serial">JSON serialized value</param>
        /// <returns>Dynamic resulting object</returns>
        private dynamic GetValueFrom(string serial)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(serial);
        }

        /// <summary>
        /// Deserialize a protobuf message from an input stream and save it in history
        /// </summary>
        /// <typeparam name="T">Type of the protobuf message to deserialize</typeparam>
        /// <param name="inStream">Input stream from which deserialize the message</param>
        /// <returns>The deserialized message</returns>
        private T GetMessage<T>(Stream inStream)
        {
            T message = ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(inStream, _prefix);

            if (message == null)
            {
                throw new InvalidDataException("ProtobufDispatcher.GetMessage<" + typeof(T).ToString() + ">: Unable to deserialize data");
            }
            _commands.Add(message);
            return message;
        }

        /// <summary>
        /// Handle command resolution by readin command on input and writing reply on output
        /// </summary>
        /// <typeparam name="Command">Type of the command to resolve</typeparam>
        /// <typeparam name="Reply">Type of the reply to send</typeparam>
        /// <param name="inStream">Input stream on which read the command</param>
        /// <param name="outStream">Output stream on which write the command</param>
        /// <param name="callback">Function that generates a reply from the command</param>
        private void ResolveCommand<Command, Reply>(Stream inStream, Stream outStream, Func<Command, Reply> callback)
        {
            Command message = GetMessage<Command>(inStream);
            Reply reply = callback(message);

            if (outStream != null)
                ProtoBuf.Serializer.SerializeWithLengthPrefix(outStream, reply, _prefix);
        }

        ///<see cref="IManager.SaveCommandsTo(string)"/>
        public void SaveCommandsTo(string filename)
        {
            List<COMMANDS> index = new List<COMMANDS>();

            //fill index with commands

            //serialise the index in the file
            //serialise each command in the file

            throw new NotImplementedException();
        }

        ///<see cref="IManager.LoadCommandsFrom(string)"/>
        public void LoadCommandsFrom(string filename)
        {
            StreamReader file = new StreamReader(filename);
            List<COMMANDS> index = ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<COMMANDS>>(file.BaseStream, _prefix);

            //for each COMMAND in the index
            //  call the right callback with file.Basestream as inStream and null as outStream

            throw new NotImplementedException();
        }

        ///<see cref="IManager.onDeclare(Stream, Stream)"/>
        public void onDeclare(Stream inStream, Stream outStream)
        {
            ResolveCommand(inStream, outStream,
                (Command.Declare message) =>
                {
                    return new Reply.EntityDeclared
                    {
                        Command = message,
                        EntityID = _controller.Declare(message.EntityType, message.ContainerID, message.Name, message.Visibility)
                    };
                });
        }

        ///<see cref="IManager.onSetVariableValue(Stream, Stream)"/>
        public void onSetVariableValue(Stream inStream, Stream outStream)
        {
            ResolveCommand(inStream, outStream,
                (Command.SetVariableValue message) =>
                {
                    _controller.SetVariableValue(message.VariableID, GetValueFrom(message.Value));
                    return new Reply.VariableValueSet
                    {
                        Command = message
                    };
                });
        }

        ///<see cref="IManager.onSetVariableType(Stream, Stream)"/>
        public void onSetVariableType(Stream inStream, Stream outStream)
        {
            ResolveCommand(inStream, outStream,
                (Command.SetVariableType message) =>
                {
                    _controller.SetVariableType(message.VariableID, message.TypeID);
                    return new Reply.VariableTypeSet
                    {
                        Command = message
                    };
                });
        }
    }
}