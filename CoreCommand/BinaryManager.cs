using CoreCommand.Command;
using CoreControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CoreCommand
{   
    /// <summary>
    /// Dispatcher that handles the command events, updating the watcher accordingly.
    /// </summary>
    public class BinaryManager : IManager
    {
        private static UInt32 MagicNumber = 0xFA7BEA57; //FATBEAST
        
        /// <summary>
        /// Controller on which dispatch command
        /// </summary>
        public Controller Controller { get; private set; } = new Controller();

        /// <summary>
        /// Path to the loaded file.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Internal history of dispatched commands => for serialisation
        /// </summary>
        private readonly List<dynamic> _commands = new List<dynamic>();

        /// <summary>
        /// Dictionnary that contains all handled commands of the manager
        /// </summary>
        private Resolver.ACommandResolver _resolver = new Resolver.V1_0_0();

        /// <summary>
        /// Dictionnary that contains commands name, synchronized with _handledCommands dictionnary
        /// </summary>
        private Dictionary<Type, String> _commandsType = new Dictionary<Type, string>();

        /// <summary>
        /// Dictionarry that associates commands name to replies name
        /// </summary>
        private Dictionary<String, String> _commandsReply = new Dictionary<string, string>();

        private readonly static Dictionary<Command.Global.SetProtocol.ProtocolValues, Action<dynamic, Stream>> Writers = new Dictionary<Command.Global.SetProtocol.ProtocolValues, Action<dynamic, Stream>>
        {
            { Command.Global.SetProtocol.ProtocolValues.BINARY, new Action<dynamic, Stream>(WriteBinaryValue) },
            { Command.Global.SetProtocol.ProtocolValues.JSON, new Action<dynamic, Stream>(WriteJsonValue) }
        };

        private readonly static Dictionary<Command.Global.SetProtocol.ProtocolValues, Func<Stream, Type, dynamic>> Readers = new Dictionary<Command.Global.SetProtocol.ProtocolValues, Func<Stream, Type, dynamic>>
        {
            { Command.Global.SetProtocol.ProtocolValues.BINARY, new Func<Stream, Type, dynamic>(ReadBinaryValue) },
            { Command.Global.SetProtocol.ProtocolValues.JSON, new Func<Stream, Type, dynamic>(ReadJsonValue) }
        };

        private Action<dynamic, Stream> SerializeAction { get; set; } = Writers[Command.Global.SetProtocol.ProtocolValues.BINARY];

        private Func<Stream, Type, dynamic> DeserializeAction { get; set; } = Readers[Command.Global.SetProtocol.ProtocolValues.BINARY];

        /// <summary>
        /// Constructor that register the core commands with the right version
        /// </summary>
        public BinaryManager()
        {
            //DECLARATOR

            RegisterCommand<Command.Declarator.Declare, Command.Declarator.Declare.Reply>               (Resolver.V1_0_0.Code, "DECLARATOR.DECLARE", "DECLARATOR.DECLARED");
            RegisterCommand<Command.Declarator.Move, EmptyReply>                                        (Resolver.V1_0_0.Code, "DECLARATOR.MOVE", "DECLARATOR.MOVED");
            RegisterCommand<Command.Declarator.Remove, Command.Declarator.Remove.Reply>                 (Resolver.V1_0_0.Code, "DECLARATOR.REMOVE", "DECLARATOR.REMOVED");
            RegisterCommand<Command.Declarator.Rename, EmptyReply>                                      (Resolver.V1_0_0.Code, "DECLARATOR.RENAME", "DECLARATOR.RENAMED");
            RegisterCommand<Command.Declarator.SetVisibility, EmptyReply>                               (Resolver.V1_0_0.Code, "DECLARATOR.SET_VISIBILITY", "DECLARATOR.VISIBILITY_SET");
            RegisterCommand<Command.Declarator.GetChildren, Command.Declarator.GetChildren.Reply>       (Resolver.V1_0_0.Code, "DECLARATOR.GET_CHILDREN", "DECLARATOR.CHILDREN_GET", false);
            
            //FUNCTION
            
            RegisterCommand<Command.Function.Call, Command.Function.Call.Reply>                         (Resolver.V1_0_0.Code, "FUNCTION.CALL", "FUNCTION.CALLED", false);
            RegisterCommand<Command.Function.AddInstruction, Command.Function.AddInstruction.Reply>     (Resolver.V1_0_0.Code, "FUNCTION.ADD_INSTRUCTION", "FUNCTION.INSTRUCTION_ADDED");
            RegisterCommand<Command.Function.RemoveInstruction, EmptyReply>                             (Resolver.V1_0_0.Code, "FUNCTION.REMOVE_INSTRUCTION", "FUNCTION.INSTRUCTION_REMOVED");
            RegisterCommand<Command.Function.SetEntryPoint, EmptyReply>                                 (Resolver.V1_0_0.Code, "FUNCTION.SET_ENTRY_POINT", "FUNCTION.ENTRY_POINT_SET");
            RegisterCommand<Command.Function.SetParameter, EmptyReply>                                  (Resolver.V1_0_0.Code, "FUNCTION.SET_PARAMETER", "FUNCTION.PARAMETER_SET");
            RegisterCommand<Command.Function.SetReturn, EmptyReply>                                     (Resolver.V1_0_0.Code, "FUNCTION.SET_RETURN", "FUNCTION.RETURN_SET");
            RegisterCommand<Command.Function.GetParams, Command.Function.GetParams.Reply>               (Resolver.V1_0_0.Code, "FUNCTION.GET_PARAMS", "FUNCTION.PARAMS_GET", false);
            RegisterCommand<Command.Function.GetReturns, Command.Function.GetReturns.Reply>             (Resolver.V1_0_0.Code, "FUNCTION.GET_RETURNS", "FUNCTION.RETURNS_GET", false);

            //FUNCTION.INSTRUCTION

            RegisterCommand<Command.Function.Instruction.LinkData, EmptyReply>                          (Resolver.V1_0_0.Code, "FUNCTION.INSTRUCTION.LINK_DATA", "FUNCTION.INSTRUCTION.DATA_LINKED");
            RegisterCommand<Command.Function.Instruction.LinkExecution, EmptyReply>                     (Resolver.V1_0_0.Code, "FUNCTION.INSTRUCTION.LINK_EXECUTION", "FUNCTION.INSTRUCTION.EXECUTION_LINKED");
            RegisterCommand<Command.Function.Instruction.SetInputValue, EmptyReply>                     (Resolver.V1_0_0.Code, "FUNCTION.INSTRUCTION.SET_INPUT_VALUE", "FUNCTION.INSTRUCTION.INPUT_VALUE_SET");
            RegisterCommand<Command.Function.Instruction.UnlinkFlow, EmptyReply>                        (Resolver.V1_0_0.Code, "FUNCTION.INSTRUCTION.UNLINK_EXECUTION", "FUNCTION.INSTRUCTION.EXECUTION_UNLINKED");
            RegisterCommand<Command.Function.Instruction.UnlinkData, EmptyReply>                        (Resolver.V1_0_0.Code, "FUNCTION.INSTRUCTION.UNLINK_DATA", "FUNCTION.INSTRUCTION.DATA_UNLINKED");

            //VARIABLE

            RegisterCommand<Command.Variable.GetType, Command.Variable.GetType.Reply>                   (Resolver.V1_0_0.Code, "VARIABLE.GET_TYPE", "VARIABLE.TYPE_GET", false);
            RegisterCommand<Command.Variable.GetValue, Command.Variable.GetValue.Reply>                 (Resolver.V1_0_0.Code, "VARIABLE.GET_VALUE", "VARIABLE.VALUE_GET", false);
            RegisterCommand<Command.Variable.SetType, EmptyReply>                                       (Resolver.V1_0_0.Code, "VARIABLE.SET_TYPE", "VARIABLE.TYPE_SET");
            RegisterCommand<Command.Variable.SetValue, EmptyReply>                                      (Resolver.V1_0_0.Code, "VARIABLE.SET_VALUE", "VARIABLE.VALUE_SET");

            //CLASS

            RegisterCommand<Command.Class.AddAttribute, EmptyReply>                                     (Resolver.V1_0_0.Code, "CLASS.ADD_ATTRIBUTE", "CLASS.ATTRIBUTE_ADDED");
            RegisterCommand<Command.Class.RemoveAttribute, EmptyReply>                                  (Resolver.V1_0_0.Code, "CLASS.REMOVE_ATTRIBUTE", "CLASS.ATTRIBUTE_REMOVED");
            RegisterCommand<Command.Class.RenameAttribute, EmptyReply>                                  (Resolver.V1_0_0.Code, "CLASS.RENAME_ATTRIBUTE", "CLASS.ATTRIBUTE_RENAMED");
            RegisterCommand<Command.Class.SetFunctionAsMember, Command.Class.SetFunctionAsMember.Reply> (Resolver.V1_0_0.Code, "CLASS.SET_FUNCTION_AS_MEMBER", "CLASS.FUNCTION_SET_AS_MEMBER");
            RegisterCommand<Command.Class.GetAttributes, Command.Class.GetAttributes.Reply>             (Resolver.V1_0_0.Code, "CLASS.GET_ATTRIBUTES", "CLASS.ATTRIBUTES_GET");

            //ENUM

            RegisterCommand<Command.Enum.GetValue, Command.Enum.GetValue.Reply>                         (Resolver.V1_0_0.Code, "ENUM.GET_VALUE", "ENUM.VALUE_GET", false);
            RegisterCommand<Command.Enum.RemoveValue, EmptyReply>                                       (Resolver.V1_0_0.Code, "ENUM.REMOVE_VALUE", "ENUM.VALUE_REMOVED");
            RegisterCommand<Command.Enum.SetType, EmptyReply>                                           (Resolver.V1_0_0.Code, "ENUM.SET_TYPE", "ENUM.TYPE_SET");
            RegisterCommand<Command.Enum.SetValue, EmptyReply >                                         (Resolver.V1_0_0.Code, "ENUM.SET_VALUE", "ENUM.VALUE_SET");
            RegisterCommand<Command.Enum.GetValues, Command.Enum.GetValues.Reply>                       (Resolver.V1_0_0.Code, "ENUM.GET_VALUES", "ENUM.VALUES_GET", false);

            //LIST

            RegisterCommand<Command.List.SetType, EmptyReply>                                           (Resolver.V1_0_0.Code, "LIST.SET_TYPE", "LIST.TYPE_SET");

            //GLOBAL

            RegisterCommand<Command.Global.CreateProject, Command.Global.CreateProject.Reply>           (Resolver.V1_0_0.Code, "GLOBAL.CREATE_PROJECT", "GLOBAL.PROJECT_CREATED", true);
            RegisterCommand<Command.Global.RemoveProject, Command.Global.RemoveProject.Reply>           (Resolver.V1_0_0.Code, "GLOBAL.REMOVE_PROJECT", "GLOBAL.PROJECT_REMOVED", true);
            RegisterCommand<Command.Global.GetProjectEntities, Command.Global.GetProjectEntities.Reply> (Resolver.V1_0_0.Code, "GLOBAL.GET_PROJECT_ENTITIES", "GLOBAL.PROJECT_ENTITIES_GET", false);
            RegisterCommand<Command.Global.Save, EmptyReply>                                            (Resolver.V1_0_0.Code, "GLOBAL.SAVE_TO", "GLOBAL.SAVED", false);
            RegisterCommand<Command.Global.Load, Command.Global.Load.Reply>                             (Resolver.V1_0_0.Code, "GLOBAL.LOAD_FROM", "GLOBAL.LOADED", true);
            RegisterCommand                                                                             (Resolver.V1_0_0.Code, "GLOBAL.RESET", "GLOBAL.RESET_DONE", false, (EmptyCommand cmd) =>
            {
                Reset();
                return new EmptyReply();
            });
            RegisterCommand<Command.Global.SetProtocol, EmptyReply>                                     (Resolver.V1_0_0.Code, "GLOBAL.SET_PROTOCOL", "GLOBAL.PROTOCOL_SET", false, (Command.Global.SetProtocol cmd) =>
            {
                SerializeAction = Writers[cmd.Protocol];
                DeserializeAction = Readers[cmd.Protocol];
                return null;
            });
            RegisterCommand<Command.Global.GetEntity, Command.Global.GetEntity.Reply>                   (Resolver.V1_0_0.Code, "GLOBAL.GET_ENTITY", "GLOBAL.ENTITY_GET", false);

            //RESSOURCE
            RegisterCommand<Command.Ressource.SetDirectory, EmptyReply>(Resolver.V1_0_0.Code, "RESSOURCE.SET_DIRECTORY", "RESSOURCE.DIRECTORY_SET", false);
        }

        private static dynamic ReadBinaryValue(Stream stream, Type type)
        {
            return BinarySerializer.Serializer.Deserialize(type, stream);
        }

        private static void WriteBinaryValue(dynamic value, Stream stream)
        {
            BinarySerializer.Serializer.Serialize(value, stream);
        }

        private static dynamic ReadJsonValue(Stream stream, Type type)
        {
            byte[] data = new byte[4096];
            StringBuilder builder = new StringBuilder();
            int read = 0;

            do
            {
                read = stream.Read(data, 0, 4096);

                if (read > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(data));
                }
            } while (read == 4096);

            return JsonConvert.DeserializeObject(builder.ToString(), type);
        }

        private static void WriteJsonValue(dynamic value, Stream stream)
        {
            string val = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            byte[] bdata = Encoding.UTF8.GetBytes(val);

            stream.Write(bdata, 0, bdata.Length);
        }

        /// <summary>
        /// Convert a JSON value to a dynamic object
        /// </summary>
        /// <param name="serial">JSON serialized value</param>
        /// <returns>Dynamic resulting object</returns>
        private dynamic GetValueFrom(string serial)
        {
            return JsonConvert.DeserializeObject(serial);
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
            T message = DeserializeAction(inStream, typeof(T));// BinarySerializer.Serializer.Deserialize<T>(inStream);//ProtoBuf.Serializer.DeserializeWithLengthPrefix<T>(inStream, _prefix);

            if (message == null)
            {
                throw new InvalidDataException($"Manager.GetMessage: Unable to deserialize data of type {typeof(T)}");
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

            Console.WriteLine("==Manager.Command== : " + JsonConvert.SerializeObject(message, Formatting.Indented));

            if (outStream != null)
            {
                SerializeAction(message, outStream);
                //BinarySerializer.Serializer.Serialize(message, outStream);
            }

            try
            {
                Reply reply = callback(message);

                Console.WriteLine("==Manager.Reply== : " + JsonConvert.SerializeObject(reply, Formatting.Indented));

                if (outStream != null && reply != null)
                {
                    SerializeAction(reply, outStream);
                    //BinarySerializer.Serializer.Serialize(reply, outStream);
                }
                return true;
            }
            catch (Exception error)
            {
                Console.Error.WriteLine("Error: " + error.Message);
                Console.Error.Write(error.StackTrace);

                if (outStream != null)
                {
                    SerializeAction(error.Message, outStream);
                    //BinarySerializer.Serializer.Serialize(error.Message, outStream);
                }

                if (save)
                {
                    Console.Error.WriteLine("Removing command from file");
                    _commands.RemoveAt(_commands.Count - 1);
                }
                return false;
            }
        }

        /// <summary>
        /// Register a specific command in the resolver
        /// </summary>
        /// <typeparam name="Command">Type of the command to register</typeparam>
        /// <typeparam name="Reply">Type of the reply associated</typeparam>
        /// <param name="version">Version of the command</param>
        /// <param name="name">Name of the command</param>
        /// <param name="replyName">Name of the reply associated</param>
        /// <param name="save">Tells if we need to save the command for file serialization (default: true)</param>
        /// <param name="callback">Function to call on resolution (optionnal)</param>
        private void RegisterCommand<Command, Reply>(String version, String name, String replyName, bool save = true, Func<Command, Reply> callback = null) where Command : ICommand<Reply>
        {
            if (callback == null)
                callback = (Command message) =>
                {
                    return message.Resolve(Controller);
                };
            _resolver.Register(version, name, (Stream inS, Stream ouS) =>
            {
                return ResolveCommand(inS, ouS, save, callback);
            });
            _commandsType[typeof(Command)] = name;
            _commandsReply[name] = replyName;
        }
        
        ///<see cref="IManager.SaveCommandsTo(string)"/>
        public void SaveCommandsTo(string filename)
        {
            if (!CallCommand(new Command.Global.Save { Filename = filename }))
            {
                throw new InvalidOperationException($"Couldn't save the project into {filename}");
            }
        }

        ///<see cref="IManager.LoadCommandsFrom(string)"/>
        public void LoadCommandsFrom(string filename)
        {
            Controller.LoadFrom(filename);
            FilePath = filename;
            //if (!CallCommand(new Command.Global.Load { Filename = filename }))
            //{
            //    throw new FileLoadException($"Unable to load the project from {filename}");
            //}

            //using (var file = new StreamReader(filename))
            //{
            //    UInt32 magic = BinarySerializer.Serializer.Deserialize<UInt32>(file.BaseStream);

            //    if (magic != MagicNumber)
            //    {
            //        file.Close();
            //        throw new InvalidOperationException("Given file \"" + filename + "\" is not a DNAI file or is corrupted");
            //    }

            //    Controller save = Controller;

            //    Reset();

            //    foreach (var command in BinarySerializer.Serializer.Deserialize<List<string>>(file.BaseStream))//ProtoBuf.Serializer.DeserializeWithLengthPrefix<List<string>>(file.BaseStream, _prefix)
            //    {
            //        if (!CallCommand(command, file.BaseStream, null))
            //            throw new InvalidOperationException("BinaryManager.LoadCommandsFrom : Error while executing command \"" + command + "\"");
            //    }

            //    FilePath = filename;

            //    save.merge(Controller);
            //    Controller = save;

            //    file.Close();
            //}
        }

        /// <see cref="IManager.CallCommand(string, Stream, Stream)"/>
        public bool CallCommand(string command, Stream inStream, Stream outStream)
        {
            return _resolver.Resolve(command, inStream, outStream);// GetCommand(command)(inStream, outStream);
        }

        public bool CallCommand<Command, Reply>(Command tosend, out Reply reply) 
            where Command : ICommand<Reply>
        {
            MemoryStream input = new MemoryStream();
            MemoryStream output = new MemoryStream();

            if (CallCommand(tosend, input, output))
            {
                output.Position = 0;
                DeserializeAction(output, typeof(Command));
                //BinarySerializer.Serializer.Deserialize<Command>(output);
                reply = DeserializeAction(output, typeof(Reply));
                //reply = BinarySerializer.Serializer.Deserialize<Reply>(output);
                return true;
            }
            output.Position = 0;
            DeserializeAction(output, typeof(Command));
            //BinarySerializer.Serializer.Deserialize<Command>(output);
            string error = DeserializeAction(output, typeof(string));
            //BinarySerializer.Serializer.Deserialize<String>(output)
            Console.Error.WriteLine(error);
            reply = default(Reply);
            return false;
        }

        public bool CallCommand<Reply>(ICommand<Reply> tosend, MemoryStream input = null, MemoryStream output = null)
        {
            if (input == null)
                input = new MemoryStream();
            if (output == null)
                output = new MemoryStream();

            //BinarySerializer.Serializer.Serialize(tosend, input);
            SerializeAction(tosend, input);
            input.Position = 0;
            if (!CallCommand(GetCommandName(tosend.GetType()), input, output))
            {
                return false;
            }
            return true;
        }
        
        /// <see cref="IManager.GetRegisteredCommands"/>
        public Dictionary<String, String> GetRegisteredCommands()
        {
            return _commandsReply;
        }

        /// <see cref="IManager.GetCommandName(Type)"/>
        public String GetCommandName(Type commandType)
        {
            if (!_commandsType.ContainsKey(commandType))
                throw new KeyNotFoundException("BinaryManager.GetCommandName : No such name registered on type " + commandType.ToString());
            return _commandsType[commandType];
        }

        /// <see cref="IManager.Reset"/>
        public void Reset()
        {
            Controller = new Controller();
            _commands.Clear();
        }
    }
}