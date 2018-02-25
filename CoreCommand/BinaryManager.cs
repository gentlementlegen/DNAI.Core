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
        private readonly Controller _controller = new Controller();

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
            RegisterCommand<AddClassAttribute, AddClassAttribute.Reply>("ADD_CLASS_ATTRIBUTE", "CLASS_ATTRIBUTE_ADDED");
            RegisterCommand<SetClassFunctionAsMember, SetClassFunctionAsMember.Reply>("ADD_CLASS_MEMBER_FUNCTION", "CLASS_MEMBER_FUNCTION_ADDED");
            RegisterCommand<AddInstruction, AddInstruction.Reply>("ADD_INSTRUCTION", "INSTRUCTION_ADDED");
            RegisterCommand<CallFunction, CallFunction.Reply>("CALL_FUNCTION", "FUNCTION_CALLED", false);
            RegisterCommand<ChangeVisibility, ChangeVisibility.Reply>("CHANGE_VISIBILITY", "ENTITY_VISIBILITY_CHANGED");
            RegisterCommand<Declare, Declare.Reply>("DECLARE", "ENTITY_DECLARED");
            RegisterCommand<GetEnumerationValue, GetEnumerationValue.Reply>("GET_ENUMERATION_VALUE", "ENUMERATION_VALUE_GET", false);
            RegisterCommand<GetVariableType, GetVariableType.Reply>("GET_VARIABLE_TYPE", "VARIABLE_TYPE_GET", false);
            RegisterCommand<GetVariableValue, GetVariableValue.Reply>("GET_VARIABLE_VALUE", "VARIABLE_VALUE_GET", false);
            RegisterCommand<LinkInstructionData, LinkInstructionData.Reply>("LINK_INSTRUCTION_DATA", "INSTRUCTION_DATA_LINKED");
            RegisterCommand<LinkInstructionExecution, LinkInstructionExecution.Reply>("LINK_INSTRUCTION_EXECUTION", "INSTRUCTION_EXECUTION_LINKED");
            RegisterCommand<Move, Move.Reply>("MOVE", "ENTITY_MOVED");
            RegisterCommand<Remove, Remove.Reply>("REMOVE", "ENTITY_REMOVED");
            RegisterCommand<RemoveClassAttribute, RemoveClassAttribute.Reply>("REMOVE_CLASS_ATTRIBUTE", "CLASS_ATTRIBUTE_REMOVED");
            RegisterCommand<RemoveEnumerationValue, RemoveEnumerationValue.Reply>("REMOVE_ENUMERATION_VALUE", "ENUMERATION_VALUE_REMOVED");
            RegisterCommand<RemoveFunctionInstruction, RemoveFunctionInstruction.Reply>("REMOVE_FUNCTION_INSTRUCTION", "FUNCTION_INSTRUCTION_REMOVED");
            RegisterCommand<Rename, Rename.Reply>("RENAME", "ENTITY_RENAMED");
            RegisterCommand<RenameClassAttribute, RenameClassAttribute.Reply>("RENAME_CLASS_ATTRIBUTE", "CLASS_ATTRIBUTE_RENAMED");
            RegisterCommand<SetContextParent, SetContextParent.Reply>("SET_CONTEXT_PARENT", "CONTEXT_PARENT_SET");
            RegisterCommand<SetEnumerationType, SetEnumerationType.Reply>("SET_ENUMERATION_TYPE", "ENUMERATION_TYPE_SET");
            RegisterCommand<SetEnumerationValue, SetEnumerationValue.Reply>("SET_ENUMERATION_VALUE", "ENUMERATION_VALUE_SET");
            RegisterCommand<SetFunctionEntryPoint, SetFunctionEntryPoint.Reply>("SET_FUNCTION_ENTRY_POINT", "FUNCTION_ENTRY_POINT_SET");
            RegisterCommand<SetFunctionParameter, SetFunctionParameter.Reply>("SET_FUNCTION_PARAMETER", "FUNCTION_PARAMETER_SET");
            RegisterCommand<SetFunctionReturn, SetFunctionReturn.Reply>("SET_FUNCTION_RETURN", "FUNCTION_RETURN_SET");
            RegisterCommand<SetInstructionInputValue, SetInstructionInputValue.Reply>("SET_INSTRUCTION_INPUT_VALUE", "INSTRUCTION_INPUT_VALUE_SET");
            RegisterCommand<SetListType, SetListType.Reply>("SET_LIST_TYPE", "LIST_TYPE_SET");
            RegisterCommand<SetVariableType, SetVariableType.Reply>("SET_VARIABLE_TYPE", "VARIABLE_TYPE_SET");
            RegisterCommand<SetVariableValue, SetVariableValue.Reply>("SET_VARIABLE_VALUE", "VARIABLE_VALUE_SET");
            RegisterCommand<UnlinkInstructionFlow, UnlinkInstructionFlow.Reply>("UNLINK_INSTRUCTION_FLOW", "INSTRUCTION_FLOW_UNLINKED");
            RegisterCommand<UnlinkInstructionInput, UnlinkInstructionInput.Reply>("UNLINK_INSTRUCTION_INPUT", "INSTRUCTION_INPUT_UNLINKED");
            RegisterCommand("SERIALIZE_TO", "SERIALIZED", false, (SerializeTo cmd) =>
            {
                SaveCommandsTo(cmd.Filename);
                return cmd.Resolve(null);
            });
            RegisterCommand("LOAD_FROM", "LOADED", false, (LoadFrom cmd) =>
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

            //Console.WriteLine("Receiving : " + typeof(Command).ToString());
            try
            {
                Reply reply = callback(message);

                //Console.WriteLine("Replying : " + typeof(Reply).ToString());
                if (outStream != null)
                {
                    BinarySerializer.Serializer.Serialize(reply, outStream);// ProtoBuf.Serializer.SerializeWithLengthPrefix(outStream, reply, _prefix);
                }
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine("Error");
                if (outStream != null)
                    BinarySerializer.Serializer.Serialize(error.Message, outStream);
                //ProtoBuf.Serializer.SerializeWithLengthPrefix(outStream, error.Message, _prefix);
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
                _controller.Reset();

                foreach (var command in BinarySerializer.Serializer.Deserialize<List<string>>(file.BaseStream))//ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<string>>(file.BaseStream, _prefix)
                {
                    if (!CallCommand(command, file.BaseStream, null))
                        throw new InvalidOperationException("BinaryManager.LoadCommandsFrom : Error while executing command \"" + command + "\"");
                }
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
    }
}