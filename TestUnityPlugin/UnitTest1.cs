using BinarySerializer;
using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Editor;
using Core.Plugin.Unity.Extensions;
using Core.Plugin.Unity.Generator;
using CoreCommand;
using CoreCommand.Command;
using CoreCommand.Command.Declarator;
using CoreCommand.Command.Function;
using CoreCommand.Command.Function.Instruction;
using CoreCommand.Command.Global;
using CoreControl;
using CorePackage.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static CoreControl.EntityFactory;
using static CoreControl.InstructionFactory;

namespace TestUnityPlugin
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void CompileTest()
        //{
        //    var c = new Compiler();

        //    var res = c.Compile();

        //    var main = res.CompiledAssembly.GetType("First.Program").GetMethod("Main");
        //    main.Invoke(null, null);
        //}

        [TestMethod]
        public void TemplateFileTest()
        {
            var t = new TemplateReader();

            var content = t.GenerateTemplateContent();
            Assert.IsNotNull(content);
        }

        [TestMethod]
        public void GenerationAndCompile()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();

            GenerateMoreOrLess();

            string code = template.GenerateTemplateContent();
            var res = compiler.Compile(code);

            var type = res.CompiledAssembly.GetType("DNAI.Behaviour.Behaviour");
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void TestUnityConverter()
        {
            var manager = new BinaryManager();
            //GenerateDulyFile();
            GenerateMoreOrLess();
            manager.LoadCommandsFrom("moreOrLess.duly");
            var unity = new DulyCodeConverter(manager);

            unity.ConvertCode();
            unity.ConvertCode();
        }

        [TestMethod]
        public void GenerationFromController()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();
            var _manager = new BinaryManager();
            var variables = new List<Entity>();
            var functions = new List<Entity>();
            var dataTypes = new List<Entity>();
            //GenerateDulyFile();
            GenerateMoreOrLess();
            _manager.LoadCommandsFrom("More Or Less.dnai");
            //GenerateMoreOrLess(_manager, out variables, out functions);

            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            foreach (var id in ids)
            {
                dataTypes.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.DATA_TYPE, id));
                variables.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, id));
                functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            }
            //variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[1]);
            //functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[1]);
            //GenerateMoreOrLess(_manager, out List<Entity> variables, out List<Entity> functions);

            string code = template.GenerateTemplateContent(_manager, variables, functions, dataTypes);
            var res = compiler.Compile(code);
            res = compiler.Compile(code);
            var type = res.CompiledAssembly.GetType("DNAI.MoreOrLess.MoreOrLess");
            Assert.IsNotNull(type);
            var func = type.GetMethod("ExecutePlay");
            Assert.IsNotNull(func);
        }

        [TestMethod]
        public void GenerationFromControllerWithFunctonIds()
        {
            var _manager = new BinaryManager();
            var functions = new List<Entity>();
            var codeConverter = new DulyCodeConverter(_manager);

            //GenerateDulyFile();
            //GenerateMoreOrLess();
            //_manager.LoadCommandsFrom("test.duly");
            _manager.LoadCommandsFrom("moreOrLess.dnai");

            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            foreach (var id in ids)
            {
                functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            }
            //functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[0]);
            codeConverter.ConvertCode(new List<int> { 0 });
            codeConverter.ConvertCode(new List<int> { 0 });
        }

        [TestMethod]
        public void TestApi()
        {
            var api = new ApiAccess();

            // Connection
            var token = api.GetToken("admin", "dnai").Result;
            api.SetAuthorization(token);

            // Get all files for a user
            List<Core.Plugin.Unity.API.File> files = null;
            files = api.GetFiles(token.user_id).Result;
            Assert.IsNotNull(files, "Files are null");

            // Get a specific file
            Core.Plugin.Unity.API.File file = api.GetFile(token.user_id, files[1]._id).Result;
            Assert.IsNotNull(file, "File was null");

            // Get content of the file
            var fileContent = api.GetFileContent(token.user_id, file._id).Result;
            Assert.IsNotNull(fileContent, "File content was null");

            //var f = new ByteArrayContent(System.IO.File.ReadAllBytes("moreOrLess.duly"));
            //f.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            //api.PostFile(new Core.Plugin.Unity.API.FileUpload { file_type_id = 1, title = "MyFile", in_store = false, file = "moreOrLess.duly" }).Wait();
        }

        [TestMethod]
        public void TestSettingsSaver()
        {
            //SettingsSaver.AddItem("test", "azerty");
            //var str = SettingsSaver.GetValue<string>("test");
            var str = "More Or-Less";
            str = str.RemoveIllegalCharacters();
            Assert.IsFalse(str.Contains(" "));
        }

        //[TestMethod]
        //public void TestVectorObject()
        //{
        //    var compiler = new Compiler();
        //    var template = new TemplateReader();
        //    var _manager = new BinaryManager();
        //    var variables = new List<Entity>();
        //    var functions = new List<Entity>();
        //    var dataTypes = new List<Entity>();

        //    _manager.LoadCommandsFrom("vertex.dnai");
        //    var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
        //    foreach (var id in ids)
        //    {
        //        dataTypes.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.DATA_TYPE, id));
        //        variables.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, id));
        //        functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
        //    }
        //    //variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[1]);
        //    //functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[1]);
        //    //GenerateMoreOrLess(_manager, out List<Entity> variables, out List<Entity> functions);

        //    string code = template.GenerateTemplateContent(_manager, variables, functions, dataTypes);
        //    var res = compiler.Compile(code);
        //    res = compiler.Compile(code);

        //    var type = res.CompiledAssembly.GetType("DNAI.Vertex.DNAIBehaviour");
        //    Assert.IsNotNull(type, "Type not found");
        //    //var func = type.GetMethod("Set");
        //    //Assert.IsNotNull(func, "func is null");
        //}

        [TestMethod]
        public void TestAstar()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();
            var _manager = new BinaryManager();
            var variables = new List<Entity>();
            var functions = new List<Entity>();
            var dataTypes = new List<Entity>();

            _manager.LoadCommandsFrom("AStarFix.dnai");
            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            foreach (var id in ids)
            {
                dataTypes.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.DATA_TYPE, id));
                variables.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, id));
                functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            }
            //variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[1]);
            //functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[1]);
            //GenerateMoreOrLess(_manager, out List<Entity> variables, out List<Entity> functions);

            string code = template.GenerateTemplateContent(_manager, variables, functions, dataTypes);
            var res = compiler.Compile(code);
            res = compiler.Compile(code);
        }

        private Reply HandleCommand<Reply, Command>(Command tohandle, CoreCommand.IManager manager)
        {
            MemoryStream inp = new MemoryStream();
            MemoryStream oup = new MemoryStream();

            Serializer.Serialize(tohandle, inp);
            inp.Position = 0;
            if (!manager.CallCommand(manager.GetCommandName(typeof(Command)), inp, oup))
            {
                oup.Position = 0;
                throw new Exception(BinarySerializer.Serializer.Deserialize<String>(oup));
            }
            oup.Position = 0;
            BinarySerializer.Serializer.Deserialize<Command>(oup);
            return BinarySerializer.Serializer.Deserialize<Reply>(oup);
        }

        private void MoreOrLessExecuter(CoreCommand.IManager manager, UInt32 funcID)
        {
            int mystery_number = 47;

            int i = 0;

            CoreCommand.Command.Function.Call command = new CoreCommand.Command.Function.Call
            {
                FuncId = funcID,
                Parameters = new Dictionary<string, string>
                {
                    { "lastResult", "2" }
                }
            };
            CoreCommand.Command.Function.Call.Reply reply;
            int given;

            do
            {
                reply = HandleCommand<CoreCommand.Command.Function.Call.Reply, CoreCommand.Command.Function.Call>(command, manager);

                given = Int32.Parse(reply.Returns["result"]);

                Console.WriteLine("IA said: " + given.ToString());

                if (given > mystery_number)
                {
                    command.Parameters["lastResult"] = "1"; //less
                    Console.WriteLine("==> It's Less");
                }
                else if (given < mystery_number)
                {
                    command.Parameters["lastResult"] = "0"; //more
                    Console.WriteLine("==> It's More");
                }
                ++i;
            } while (given != mystery_number && i < 10);

            Assert.IsTrue(given == mystery_number);

            Console.WriteLine("AI found the mystery number " + mystery_number + " in " + i + " hits");
        }

        public void GenerateMoreOrLess()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();
            MemoryStream commands = new MemoryStream();
            MemoryStream output = new MemoryStream();
            uint integer = (uint)EntityFactory.BASE_ID.INTEGER_TYPE;

            Declare.Reply moreOrLessContext = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.CONTEXT,
                    ContainerID = 0,
                    Name = "moreOrLess",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);

            //int min = 0;
            Declare.Reply minVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "min",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = minVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = minVariable.EntityID,
                    Value = "0"
                }, manager);

            //int max = 100;
            Declare.Reply maxVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "max",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = maxVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = maxVariable.EntityID,
                    Value = "100"
                }, manager);

            //int lastGiven = -1;
            Declare.Reply lastGivenVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "lastGiven",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = lastGivenVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = lastGivenVariable.EntityID,
                    Value = "-1"
                }, manager);

            /*
             * enum COMPARISON
             * {
             *      MORE = 0,
             *      LESS = 1,
             *      NONE = 2
             * }
             */
            Declare.Reply COMPARISONenum = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.ENUM_TYPE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "COMPARISON",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "MORE",
                    Value = "0"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "LESS",
                    Value = "1"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "NONE",
                    Value = "2"
                }, manager);

            /*
             * COMPARISON Play(COMPARISON lastResult = NONE);
             */
            Declare.Reply playFunction = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.FUNCTION,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "Play",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            Declare.Reply play_lastResultVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "lastResult",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.SetParameter>(
                new CoreCommand.Command.Function.SetParameter
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "lastResult"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = play_lastResultVariable.EntityID,
                    TypeID = COMPARISONenum.EntityID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = play_lastResultVariable.EntityID,
                    Value = "2"
                }, manager);
            Declare.Reply play_resultVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "result",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.SetReturn>(
                new CoreCommand.Command.Function.SetReturn
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "result"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = play_resultVariable.EntityID,
                    TypeID = integer
                }, manager);

            AddInstruction.Reply splitCOMPARISON = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ENUM_SPLITTER,
                    Arguments = new List<uint> { COMPARISONenum.EntityID }
                }, manager);
            AddInstruction.Reply getLastResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_lastResultVariable.EntityID }
                }, manager);

            //if (lastResult == COMPARISON::MORE)
            AddInstruction.Reply lr_eq_more = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            AddInstruction.Reply if_lr_eq_more = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_more.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_more.InstructionID,
                    InputName = "condition"
                }, manager);

            //min = lastGiven
            AddInstruction.Reply getLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            AddInstruction.Reply setMin = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = setMin.InstructionID,
                    InputName = "value"
                }, manager);

            //if (lastResult == COMPARISON::LESS)
            AddInstruction.Reply lr_eq_less = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "LESS",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            AddInstruction.Reply if_lr_eq_less = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_less.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_less.InstructionID,
                    InputName = "condition"
                }, manager);

            //max = lastGiven
            AddInstruction.Reply set_max = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = set_max.InstructionID,
                    InputName = "value"
                }, manager);

            //min / 2
            AddInstruction.Reply getMin = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            AddInstruction.Reply minHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMin.InstructionID,
                    OutputName = "reference",
                    ToId = minHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, SetInputValue>(
                new SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = minHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //max / 2
            AddInstruction.Reply getMax = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            AddInstruction.Reply maxHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMax.InstructionID,
                    OutputName = "reference",
                    ToId = maxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, SetInputValue>(
                new SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = maxHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //min / 2 + max / 2
            AddInstruction.Reply minHalfPlusMaxHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = maxHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //result = min / 2 + max / 2
            AddInstruction.Reply setResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalfPlusMaxHalf.InstructionID,
                    OutputName = "result",
                    ToId = setResult.InstructionID,
                    InputName = "value"
                }, manager);

            //result == lastGiven
            AddInstruction.Reply getResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            AddInstruction.Reply resEqLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (result == lastGiven)
            AddInstruction.Reply ifResEqLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resEqLastGiven.InstructionID,
                    OutputName = "result",
                    ToId = ifResEqLastGiven.InstructionID,
                    InputName = "condition"
                }, manager);

            //lastResult == MORE
            AddInstruction.Reply lastResultEqMore = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (lastResult == MORE)
            AddInstruction.Reply ifLastResultEqMore = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lastResultEqMore.InstructionID,
                    OutputName = "result",
                    ToId = ifLastResultEqMore.InstructionID,
                    InputName = "condition"
                }, manager);

            //result + 1
            AddInstruction.Reply resultPP = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultPP.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, SetInputValue>(
                new SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultPP.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result + 1
            AddInstruction.Reply setResultPP = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultPP.InstructionID,
                    OutputName = "result",
                    ToId = setResultPP.InstructionID,
                    InputName = "value"
                }, manager);

            //result - 1
            AddInstruction.Reply resultMM = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SUB,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultMM.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, SetInputValue>(
                new SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultMM.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result - 1
            AddInstruction.Reply setResultMM = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultMM.InstructionID,
                    OutputName = "result",
                    ToId = setResultMM.InstructionID,
                    InputName = "value"
                }, manager);

            //lastGiven = result
            AddInstruction.Reply setLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, LinkData>(
                new LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = setLastGiven.InstructionID,
                    InputName = "value"
                }, manager);

            /*
             * if (lastResult == MORE)
             * {
             *      min = lastGiven
             * }
             * else
             * {
             *      if (lastResult == LESS)
             *      {
             *          max = lastGiven
             *      }
             * }
             * 
             * result = min / 2 + max / 2
             * 
             * if (number == lastGiven)
             * {
             *      if (lastResult == MORE)
             *      {
             *          number = number + 1
             *      }
             *      else
             *      {
             *          number = number - 1
             *      }
             * }
             * 
             * lastGiven = number
             */

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setMin.InstructionID
                }, manager);
            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 1, //on false
                    ToId = if_lr_eq_less.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setMin.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 0, //on true
                    ToId = set_max.InstructionID
                }, manager);
            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = set_max.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResult.InstructionID,
                    OutIndex = 0,
                    ToId = ifResEqLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 0, //on true
                    ToId = ifLastResultEqMore.InstructionID
                }, manager);
            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setResultPP.InstructionID
                }, manager);
            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResultMM.InstructionID
                }, manager);

            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultPP.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);
            HandleCommand<EmptyReply, LinkExecution>(
                new LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultMM.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, SetEntryPoint>(
                new SetEntryPoint
                {
                    FunctionId = playFunction.EntityID,
                    Instruction = if_lr_eq_more.InstructionID
                }, manager);

            MoreOrLessExecuter(manager, playFunction.EntityID);

            HandleCommand<EmptyReply, Save>(new Save
            {
                Filename = "moreOrLess.duly"
            }, manager);

            CoreCommand.IManager witness = new CoreCommand.BinaryManager();

            HandleCommand<EmptyReply, Load>(new Load
            {
                Filename = "moreOrLess.duly"
            }, witness);

            MoreOrLessExecuter(witness, playFunction.EntityID);
        }
    }
}