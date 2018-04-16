using CoreCommand.Command;
using CoreControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CoreCommand
{
    /// <summary>
    /// Dispatcher that handles the command events, updating the watcher accordingly.
    /// </summary>
    public class BinaryManager : IManager
    {
        /// <summary>
        /// Controller on which dispatch command
        /// </summary>
        private Controller _controller = new Controller();

        /// <summary>
        /// Internal history of dispatched commands => for serialisation
        /// </summary>
        private readonly List<dynamic> _commands = new List<dynamic>();

        /// <summary>
        /// Dictionnary that contains all handled commands of the manager
        /// </summary>
        private Dictionary<String, Func<Stream, Stream, bool>> _handledCommands = new Dictionary<string, Func<Stream, Stream, bool>>();

        /// <summary>
        /// Dictionnary that contains commands name, synchronized with _handledCommands dictionnary
        /// </summary>
        private Dictionary<Type, String> _commandsType = new Dictionary<Type, string>();

        /// <summary>
        /// Dictionarry that associates commands name to replies name
        /// </summary>
        private Dictionary<String, String> _commandsReply = new Dictionary<string, string>();
        
        public BinaryManager()
        {
            //DECLARATOR

            RegisterCommand<Command.Declarator.Declare, Command.Declarator.Declare.Reply>("DECLARATOR.DECLARE", "DECLARATOR.DECLARED");
            RegisterCommand<Command.Declarator.Move, EmptyReply>("DECLARATOR.MOVE", "DECLARATOR.MOVED");
            RegisterCommand<Command.Declarator.Remove, Command.Declarator.Remove.Reply>("DECLARATOR.REMOVE", "DECLARATOR.REMOVED");
            RegisterCommand<Command.Declarator.Rename, EmptyReply>("DECLARATOR.RENAME", "DECLARATOR.RENAMED");
            RegisterCommand<Command.Declarator.SetVisibility, EmptyReply>("DECLARATOR.SET_VISIBILITY", "DECLARATOR.VISIBILITY_SET");
            
            //FUNCTION
            
            RegisterCommand<Command.Function.Call, Command.Function.Call.Reply>("FUNCTION.CALL", "FUNCTION.CALLED", false);
            RegisterCommand<Command.Function.AddInstruction, Command.Function.AddInstruction.Reply>("FUNCTION.ADD_INSTRUCTION", "FUNCTION.INSTRUCTION_ADDED");
            RegisterCommand<Command.Function.RemoveInstruction, EmptyReply>("FUNCTION.REMOVE_INSTRUCTION", "FUNCTION.INSTRUCTION_REMOVED");
            RegisterCommand<Command.Function.SetEntryPoint, EmptyReply>("FUNCTION.SET_ENTRY_POINT", "FUNCTION.ENTRY_POINT_SET");
            RegisterCommand<Command.Function.SetParameter, EmptyReply>("FUNCTION.SET_PARAMETER", "FUNCTION.PARAMETER_SET");
            RegisterCommand<Command.Function.SetReturn, EmptyReply>("FUNCTION.SET_RETURN", "FUNCTION.RETURN_SET");

            //FUNCTION.INSTRUCTION

            RegisterCommand<Command.Function.Instruction.LinkData, EmptyReply>("FUNCTION.INSTRUCTION.LINK_DATA", "FUNCTION.INSTRUCTION.DATA_LINKED");
            RegisterCommand<Command.Function.Instruction.LinkExecution, EmptyReply>("FUNCTION.INSTRUCTION.LINK_EXECUTION", "FUNCTION.INSTRUCTION.EXECUTION_LINKED");
            RegisterCommand<Command.Function.Instruction.SetInputValue, EmptyReply>("FUNCTION.INSTRUCTION.SET_INPUT_VALUE", "FUNCTION.INSTRUCTION.INPUT_VALUE_SET");
            RegisterCommand<Command.Function.Instruction.UnlinkFlow, EmptyReply>("FUNCTION.INSTRUCTION.UNLINK_EXECUTION", "FUNCTION.INSTRUCTION.EXECUTION_UNLINKED");
            RegisterCommand<Command.Function.Instruction.UnlinkData, EmptyReply>("FUNCTION.INSTRUCTION.UNLINK_DATA", "FUNCTION.INSTRUCTION.DATA_UNLINKED");

            //VARIABLE

            RegisterCommand<Command.Variable.GetType, Command.Variable.GetType.Reply>("VARIABLE.GET_TYPE", "VARIABLE.TYPE_GET", false);
            RegisterCommand<Command.Variable.GetValue, Command.Variable.GetValue.Reply>("VARIABLE.GET_VALUE", "VARIABLE.VALUE_GET", false);
            RegisterCommand<Command.Variable.SetType, EmptyReply>("VARIABLE.SET_TYPE", "VARIABLE.TYPE_SET");
            RegisterCommand<Command.Variable.SetValue, EmptyReply>("VARIABLE.SET_VALUE", "VARIABLE.VALUE_SET");

            //CLASS

            RegisterCommand<Command.Class.AddAttribute, EmptyReply>("CLASS.ADD_ATTRIBUTE", "CLASS.ATTRIBUTE_ADDED");
            RegisterCommand<Command.Class.RemoveAttribute, EmptyReply>("CLASS.REMOVE_ATTRIBUTE", "CLASS.ATTRIBUTE_REMOVED");
            RegisterCommand<Command.Class.RenameAttribute, EmptyReply>("CLASS.RENAME_ATTRIBUTE", "CLASS.ATTRIBUTE_RENAMED");
            RegisterCommand<Command.Class.SetFunctionAsMember, Command.Class.SetFunctionAsMember.Reply>("CLASS.SET_FUNCTION_AS_MEMBER", "CLASS.FUNCTION_SET_AS_MEMBER");

            //ENUM

            RegisterCommand<Command.Enum.GetValue, Command.Enum.GetValue.Reply>("ENUM.GET_VALUE", "ENUM.VALUE_GET", false);
            RegisterCommand<Command.Enum.RemoveValue, EmptyReply>("ENUM.REMOVE_VALUE", "ENUM.VALUE_REMOVED");
            RegisterCommand<Command.Enum.SetType, EmptyReply>("ENUM.SET_TYPE", "ENUM.TYPE_SET");
            RegisterCommand<Command.Enum.SetValue, EmptyReply >("ENUM.SET_VALUE", "ENUM.VALUE_SET");

            //LIST

            RegisterCommand<Command.List.SetType, EmptyReply>("LIST.SET_TYPE", "LIST.TYPE_SET");

            //OTHER

            RegisterCommand<Command.Project.Create, Command.Project.Create.Reply>("PROJECT.CREATE", "PROJECT.CREATED", true);
            RegisterCommand<Command.Project.Remove, Command.Project.Remove.Reply>("PROJECT.REMOVE", "PROJECT.REMOVED", true);
            RegisterCommand("PROJECT.SAVE", "PROJECT.SAVED", false, (Command.Project.Save cmd) =>
            {
                SaveCommandsTo(cmd.Filename);
                return cmd.Resolve(null);
            });
            RegisterCommand("PROJECT.LOAD", "PROJECT.LOADED", true, (Command.Project.Load cmd) =>
            {
                LoadCommandsFrom(cmd.Filename);
                return cmd.Resolve(null);
            });
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
        /// Get json serialized data from dynamic value
        /// </summary>
        /// <param name="value">Dynamic value to serialize</param>
        /// <returns></returns>
        private string GetSerialFrom(dynamic value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Deserialize a protobuf message from an input stream and save it in history
        /// </summary>
        /// <typeparam name="T">Type of the protobuf message to deserialize</typeparam>
        /// <param name="inStream">Input stream from which deserialize the message</param>
        /// <returns>The deserialized message</returns>
        private T GetMessage<T>(Stream inStream, bool save)
        {
            T message = BinarySerializer.Serializer.Deserialize<T>(inStream);//ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(inStream, _prefix);

            if (message == null)
            {
                throw new InvalidDataException("ProtobufDispatcher.GetMessage<" + typeof(T) + ">: Unable to deserialize data");
            }
            if (save)
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
        private bool ResolveCommand<Command, Reply>(Stream inStream, Stream outStream, bool save, Func<Command, Reply> callback) where Command : ICommand<Reply>
        {
            Command message = GetMessage<Command>(inStream, save);

            if (outStream != null)
                BinarySerializer.Serializer.Serialize(message, outStream);

            try
            {
                Reply reply = callback(message);
                
                if (outStream != null && reply != null)
                {
                    BinarySerializer.Serializer.Serialize(reply, outStream);
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
                Console.Write(error.StackTrace);

                if (outStream != null)
                    BinarySerializer.Serializer.Serialize(error.Message, outStream);
                return false;
            }
        }

        private void RegisterCommand<Command, Reply>(String name, String replyName, bool save = true, Func<Command, Reply> callback = null) where Command : ICommand<Reply>
        {
            if (callback == null)
                callback = (Command message) =>
                {
                    return message.Resolve(_controller);
                };
            _handledCommands[name] = (Stream inS, Stream ouS) =>
            {
                return ResolveCommand(inS, ouS, save, callback);
            };
            _commandsType[typeof(Command)] = name;
            _commandsReply[name] = replyName;
        }

        ///<see cref="IManager.SaveCommandsTo(string)"/>
        public void SaveCommandsTo(string filename)
        {
            var types = new List<string>();

            using (var stream = File.Create(filename))
            {
                foreach (var command in _commands)
                {
                    if (!_commandsType.ContainsKey(command.GetType()))
                        throw new InvalidDataException("BinaryManager.SaveCommandsTo : Given command \"" + command.GetType().ToString() + "\" is not registered");
                    types.Add(_commandsType[command.GetType()]);
                }

                BinarySerializer.Serializer.Serialize(types, stream);

                foreach (var command in _commands)
                {
                    BinarySerializer.Serializer.Serialize(command, stream);
                }
            }
        }

        ///<see cref="IManager.LoadCommandsFrom(string)"/>
        public void LoadCommandsFrom(string filename)
        {
            using (var file = new StreamReader(filename))
            {
                Controller save = _controller;

                Reset();

                foreach (var command in BinarySerializer.Serializer.Deserialize<List<string>>(file.BaseStream))//ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<string>>(file.BaseStream, _prefix)
                {
                    if (!CallCommand(command, file.BaseStream, null))
                        throw new InvalidOperationException("BinaryManager.LoadCommandsFrom : Error while executing command \"" + command + "\"");
                }

                save.merge(_controller);
                _controller = save;
            }
        }

        public bool CallCommand(string command, Stream inStream, Stream outStream)
        {
            return GetCommand(command)(inStream, outStream);
        }

        public Func<Stream, Stream, bool> GetCommand(String name)
        {
            if (!_handledCommands.ContainsKey(name))
                throw new InvalidDataException("BinaryManager.GetCommand : Given command \"" + name + "\" is not registered");
            return _handledCommands[name];
        }
        
        public Dictionary<String, String> GetRegisteredCommands()
        {
            return _commandsReply;
        }

        public String GetCommandName(Type commandType)
        {
            if (!_commandsType.ContainsKey(commandType))
                throw new KeyNotFoundException("BinaryManager.GetCommandName : No such name registered on type " + commandType.ToString());
            return _commandsType[commandType];
        }

        public void Reset()
        {
            _controller = new Controller();
        }
    }
}