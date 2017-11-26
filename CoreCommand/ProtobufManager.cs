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
        /// Dictionary for matching classes Guid to handling callbacks.
        /// </summary>
        //private readonly Dictionary<Guid, Action<Stream, Stream>> _actions = new Dictionary<Guid, Action<Stream, Stream>>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProtobufManager()
        {
            //_actions.Add(typeof(Command.Declare).GUID, OnDeclare);
            //_actions.Add(typeof(Command.SetVariableType).GUID, OnSetVariableType);
            //_actions.Add(typeof(Command.SetVariableValue).GUID, OnSetVariableValue);
        }

        /// <summary>
        /// Constructor to customize protobuf prefix
        /// </summary>
        /// <param name="prefix">Serialisation prefix for protobuf</param>
        public ProtobufManager(ProtoBuf.PrefixStyle prefix) : this()
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
                throw new InvalidDataException("ProtobufDispatcher.GetMessage<" + typeof(T) + ">: Unable to deserialize data");
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
            //List<COMMANDS> index = new List<COMMANDS>();
            var types = new List<string>();

            using (var stream = File.Create(filename))
            {
                foreach (var command in _commands)
                {
                    types.Add(command.GetType().AssemblyQualifiedName);
                }
                ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, types, _prefix);
                foreach (var command in _commands)
                {
                    ProtoBuf.Serializer.SerializeWithLengthPrefix(stream, command, _prefix);
                }
            }
        }

        ///<see cref="IManager.LoadCommandsFrom(string)"/>
        public void LoadCommandsFrom(string filename)
        {
            using (var file = new StreamReader(filename))
            {
                _controller.Reset();
                //List<COMMANDS> index = ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<COMMANDS>>(file.BaseStream, _prefix);

                foreach (var command in ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<string>>(file.BaseStream, _prefix))
                {
                    var t = Type.GetType(command);
                    try
                    {
                        GetType().GetMethod("On" + t.Name).Invoke(this, new object[] { file.BaseStream, null });
                    }
                    catch
                    {
                        Console.WriteLine($"Could not Invoke the Command Callback (does the method <On${t.Name}> exists ?)");
                    }
                }
            }
        }

        ///<see cref="IManager.OnDeclare(Stream, Stream)"/>
        public void OnDeclare(Stream inStream, Stream outStream)
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

        ///<see cref="IManager.OnSetVariableValue(Stream, Stream)"/>
        public void OnSetVariableValue(Stream inStream, Stream outStream)
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

        ///<see cref="IManager.OnSetVariableType(Stream, Stream)"/>
        public void OnSetVariableType(Stream inStream, Stream outStream)
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

        public void OnRemove(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnMove(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnChangeVisibility(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnGetVariableValue(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetContextParent(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetEnumerationType(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetEnumerationValue(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnGetEnumerationValue(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnRemoveEnumerationValue(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnAddClassAttribute(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnRenameClassAttribute(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnRemoveClassAttribute(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnAddClassMemberFunction(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetListType(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnCallFunction(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetFunctionParameter(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetFunctionReturn(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetFunctionEntryPoint(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnRemoveFunctionInstruction(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnAddInstruction(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnLinkInstructionExecution(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnLinkInstructionData(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnSetInstructionInputValuO(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnUnlinkInstructionFlow(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }

        public void OnUnlinkInstructionInput(Stream inStream, Stream outStream)
        {
            throw new NotImplementedException();
        }
    }
}