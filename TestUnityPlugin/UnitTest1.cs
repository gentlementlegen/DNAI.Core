using Core.Plugin.Unity.Generator;
using CoreCommand;
using CorePackage.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using static CoreControl.EntityFactory;
using static CoreControl.InstructionFactory;

namespace TestUnityPlugin
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CompileTest()
        {
            var c = new Compiler();

            var res = c.Compile();

            var main = res.CompiledAssembly.GetType("First.Program").GetMethod("Main");
            main.Invoke(null, null);
        }

        [TestMethod]
        public void TemplateFileTest()
        {
            var t = new TemplateReader();

            var content = t.GenerateTemplateContent();
        }

        [TestMethod]
        public void GenerationAndCompile()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();

            string code = template.GenerateTemplateContent();
            var res = compiler.Compile(code);

            var type = res.CompiledAssembly.GetType("DulyBehaviour");
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void GenerationFromController()
        {
            var compiler = new Compiler();
            var template = new TemplateReader();
            var _manager = new ProtobufManager();
            var variables = new List<Entity>();
            var functions = new List<Entity>();
            GenerateDulyFile();
            _manager.LoadCommandsFrom("test.duly");
            //GenerateMoreOrLess(_manager, out variables, out functions);

            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[0]);
            functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[0]);
            //GenerateMoreOrLess(_manager, out List<Entity> variables, out List<Entity> functions);

            string code = template.GenerateTemplateContent(_manager, variables, functions);
            var res = compiler.Compile(code);

            var type = res.CompiledAssembly.GetType("DulyBehaviour");
            Assert.IsNotNull(type);
        }

        private void TestCommand<Command, Reply>(CoreCommand.IManager manager, Command toserial, Action<Stream, Stream> toCall, Action<Command, Reply> check)
        {
            Stream instream = new MemoryStream();
            Stream outStream = new MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(instream, toserial, ProtoBuf.PrefixStyle.Base128);
            instream.Flush();
            instream.Position = 0;

            toCall(instream, outStream);
            //instream.Dispose();

            outStream.Position = 0;
            var t = typeof(Reply);
            Reply reply = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Reply>(outStream, ProtoBuf.PrefixStyle.Base128);
            outStream.Flush();
            //outStream.Dispose();

            check(toserial, reply);
        }

        public void GenerateDulyFile()
        {
            CoreCommand.IManager dispatcher = new CoreCommand.ProtobufManager();

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 0,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "toto",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnDeclare,
                (CoreCommand.Command.Declare command, CoreCommand.Reply.EntityDeclared reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ContainerID == command.ContainerID
                       && reply.Command.EntityType == command.EntityType
                       && reply.Command.Name == command.Name
                       && reply.Command.Visibility == command.Visibility
                       && reply.EntityID == 6);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 0,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "MyVariable",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnDeclare,
                (CoreCommand.Command.Declare command, CoreCommand.Reply.EntityDeclared reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ContainerID == command.ContainerID
                       && reply.Command.EntityType == command.EntityType
                       && reply.Command.Name == command.Name
                       && reply.Command.Visibility == command.Visibility
                       && reply.EntityID == 7);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 0,
                    EntityType = CoreControl.EntityFactory.ENTITY.FUNCTION,
                    Name = "MyFunction",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnDeclare,
                (CoreCommand.Command.Declare command, CoreCommand.Reply.EntityDeclared reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ContainerID == command.ContainerID
                       && reply.Command.EntityType == command.EntityType
                       && reply.Command.Name == command.Name
                       && reply.Command.Visibility == command.Visibility
                       && reply.EntityID == 8);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 8,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "param1",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnDeclare,
                (CoreCommand.Command.Declare command, CoreCommand.Reply.EntityDeclared reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ContainerID == command.ContainerID
                       && reply.Command.EntityType == command.EntityType
                       && reply.Command.Name == command.Name
                       && reply.Command.Visibility == command.Visibility
                       && reply.EntityID == 9);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 8,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "return1",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnDeclare,
                (CoreCommand.Command.Declare command, CoreCommand.Reply.EntityDeclared reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.ContainerID == command.ContainerID
                        && reply.Command.EntityType == command.EntityType
                        && reply.Command.Name == command.Name
                        && reply.Command.Visibility == command.Visibility
                        && reply.EntityID == 10);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 6,
                    TypeID = 2
                },
                dispatcher.OnSetVariableType,
                (CoreCommand.Command.SetVariableType message, CoreCommand.Reply.VariableTypeSet reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.VariableID == message.VariableID
                        && reply.Command.TypeID == message.TypeID);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 6,
                    Value = "42"
                },
                dispatcher.OnSetVariableValue,
                (CoreCommand.Command.SetVariableValue message, CoreCommand.Reply.VariableValueSet reply) =>
                {
                    Assert.IsTrue(
                        message.VariableID == reply.Command.VariableID
                        && message.Value == reply.Command.Value
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 7,
                    TypeID = 2
                },
                dispatcher.OnSetVariableType,
                (CoreCommand.Command.SetVariableType message, CoreCommand.Reply.VariableTypeSet reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.VariableID == message.VariableID
                        && reply.Command.TypeID == message.TypeID);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 7,
                    Value = "42"
                },
                dispatcher.OnSetVariableValue,
                (CoreCommand.Command.SetVariableValue message, CoreCommand.Reply.VariableValueSet reply) =>
                {
                    Assert.IsTrue(
                        message.VariableID == reply.Command.VariableID
                        && message.Value == reply.Command.Value
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 9,
                    TypeID = 2
                },
                dispatcher.OnSetVariableType,
                (CoreCommand.Command.SetVariableType message, CoreCommand.Reply.VariableTypeSet reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.VariableID == message.VariableID
                        && reply.Command.TypeID == message.TypeID);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 9,
                    Value = "666"
                },
                dispatcher.OnSetVariableValue,
                (CoreCommand.Command.SetVariableValue message, CoreCommand.Reply.VariableValueSet reply) =>
                {
                    Assert.IsTrue(
                        message.VariableID == reply.Command.VariableID
                        && message.Value == reply.Command.Value
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 10,
                    TypeID = 2
                },
                dispatcher.OnSetVariableType,
                (CoreCommand.Command.SetVariableType message, CoreCommand.Reply.VariableTypeSet reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.VariableID == message.VariableID
                        && reply.Command.TypeID == message.TypeID);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 10,
                    Value = "-1"
                },
                dispatcher.OnSetVariableValue,
                (CoreCommand.Command.SetVariableValue message, CoreCommand.Reply.VariableValueSet reply) =>
                {
                    Assert.IsTrue(
                        message.VariableID == reply.Command.VariableID
                        && message.Value == reply.Command.Value
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.ChangeVisibility
                {
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    ContainerID = 0,
                    Name = "toto",
                    NewVisi = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.OnChangeVisibility,
                (CoreCommand.Command.ChangeVisibility message, CoreCommand.Reply.ChangeVisibility reply) =>
                {
                    Assert.IsTrue(
                        message.Name == reply.Command.Name
                        && message.ContainerID == reply.Command.ContainerID
                        && message.EntityType == reply.Command.EntityType
                        && message.NewVisi == reply.Command.NewVisi
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.GetVariableValue
                {
                    VariableId = 6
                },
                dispatcher.OnGetVariableValue,
                (CoreCommand.Command.GetVariableValue message, CoreCommand.Reply.VariableValueGet reply) =>
                {
                    Assert.IsTrue(
                        message.VariableId == reply.Command.VariableId
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.Remove
                {
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    ContainerID = 0,
                    Name = "toto"
                },
                dispatcher.OnRemove,
                (CoreCommand.Command.Remove message, CoreCommand.Reply.Remove reply) =>
                {
                    Assert.IsTrue(
                        message.Name == reply.Command.Name
                        && message.ContainerID == reply.Command.ContainerID
                        && message.EntityType == reply.Command.EntityType
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetFunctionParameter
                {
                    ExternalVarName = "param1",
                    FuncId = 8
                },
                dispatcher.OnSetFunctionParameter,
                (CoreCommand.Command.SetFunctionParameter command, CoreCommand.Reply.SetFunctionParameter reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ExternalVarName == command.ExternalVarName
                       && reply.Command.FuncId == command.FuncId);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetFunctionReturn
                {
                    ExternalVarName = "return1",
                    FuncId = 8
                },
                dispatcher.OnSetFunctionReturn,
                (CoreCommand.Command.SetFunctionReturn command, CoreCommand.Reply.SetFunctionReturn reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ExternalVarName == command.ExternalVarName
                       && reply.Command.FuncId == command.FuncId);
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.AddInstruction
                {
                    FunctionID = 8,
                    Arguments = new System.Collections.Generic.List<uint> { 8 },
                    ToCreate = CoreControl.InstructionFactory.INSTRUCTION_ID.IF
                },
                dispatcher.OnAddInstruction,
                (CoreCommand.Command.AddInstruction message, CoreCommand.Reply.AddInstruction reply) =>
                {
                    Assert.IsTrue(
                        message.FunctionID == reply.Command.FunctionID
                        && message.Arguments.Count == reply.Command.Arguments.Count
                        && message.ToCreate == reply.Command.ToCreate
                        );
                });

            TestCommand(
                dispatcher,
                new CoreCommand.Command.SetFunctionEntryPoint
                {
                    FunctionId = 8,
                    Instruction = 0
                },
                dispatcher.OnSetFunctionEntryPoint,
                (CoreCommand.Command.SetFunctionEntryPoint message, CoreCommand.Reply.SetFunctionEntryPoint reply) =>
                {
                    Assert.IsTrue(
                        message.FunctionId == reply.Command.FunctionId
                        && message.Instruction == reply.Command.Instruction
                        );
                });

            dispatcher.SaveCommandsTo("test.duly");
        }

        private void GenerateController(ProtobufManager manager, out List<Entity> variables)
        {
            CoreControl.Controller controller = manager.Controller;
            uint integer = (uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE;
            uint floating = (uint)CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE;

            uint ctx = controller.Declare(ENTITY.CONTEXT_D, 0, "toto", VISIBILITY.PRIVATE);
            uint fnt = controller.Declare(ENTITY.FUNCTION, 0, "toto", VISIBILITY.PRIVATE);
            uint var = controller.Declare(ENTITY.VARIABLE, 0, "toto", VISIBILITY.PRIVATE);
            uint enu = controller.Declare(ENTITY.ENUM_TYPE, 0, "toto", VISIBILITY.PRIVATE);
            uint obj = controller.Declare(ENTITY.OBJECT_TYPE, 0, "tata", VISIBILITY.PRIVATE);
            uint lst = controller.Declare(ENTITY.LIST_TYPE, 0, "tutu", VISIBILITY.PRIVATE);

            //context

            controller.SetContextParent(ctx, 0);

            //variable

            controller.SetVariableType(var, integer);
            Assert.IsTrue(controller.GetVariableType(var) == integer);
            controller.SetVariableValue(var, 42);
            Assert.IsTrue(controller.GetVariableValue(var) == 42);

            //enum

            controller.SetEnumerationType(enu, floating);
            controller.SetEnumerationValue(enu, "TUTU", 43.2);
            Assert.IsTrue(controller.GetEnumerationValue(enu, "TUTU") == 43.2);
            controller.RemoveEnumerationValue(enu, "TUTU");

            //class

            controller.AddClassAttribute(obj, "posX", integer, VISIBILITY.PUBLIC);
            controller.AddClassAttribute(obj, "posY", integer, VISIBILITY.PUBLIC);
            controller.RenameClassAttribute(obj, "posX", "posZ");
            controller.RemoveClassAttribute(obj, "posY");

            //uncomment it when object will be implemented

            //controller.addClassMemberFunction(obj, "Unitarize", AccessMode.EXTERNAL);

            //list

            controller.SetListType(lst, floating);

            //function

            controller.SetVariableType(var, floating);
            uint entry = controller.AddInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { var });
            controller.SetInstructionInputValue(fnt, entry, "value", 3.14);
            controller.SetFunctionEntryPoint(fnt, entry);

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { });

            Assert.IsTrue(controller.GetVariableValue(var) == 3.14);

            uint val = controller.Declare(ENTITY.VARIABLE, fnt, "value", VISIBILITY.PUBLIC);
            controller.SetFunctionParameter(fnt, "value");
            controller.SetVariableType(val, floating);

            uint get_value = controller.AddInstruction(fnt, INSTRUCTION_ID.GETTER, new List<uint> { val });
            controller.LinkInstructionData(fnt, get_value, "reference", entry, "value");

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { { "value", 42.3 } });

            Assert.IsTrue(controller.GetVariableValue(var) == 42.3);

            uint res = controller.Declare(ENTITY.VARIABLE, fnt, "res", VISIBILITY.PUBLIC);
            controller.SetFunctionReturn(fnt, "res");
            controller.SetVariableType(res, floating);

            controller.RemoveFunctionInstruction(fnt, entry);
            entry = controller.AddInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { res });
            controller.SetFunctionEntryPoint(fnt, entry);

            controller.LinkInstructionData(fnt, get_value, "reference", entry, "value");

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { { "value", 56.3 } });

            Assert.IsTrue(controller.GetVariableValue(res) == 56.3);

            controller.UnlinkInstructionInput(fnt, entry, "value");
            controller.SetInstructionInputValue(fnt, entry, "value", 71.2);

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { { "value", 31.2 } });

            Assert.IsTrue(controller.GetVariableValue(res) == 71.2);

            uint new_set = controller.AddInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { val });
            controller.LinkInstructionData(fnt, get_value, "reference", new_set, "value");
            controller.LinkInstructionExecution(fnt, entry, 0, new_set);

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { { "value", 32.2 } });

            Assert.IsTrue(controller.GetVariableValue(val) == 32.2);

            controller.UnlinkInstructionFlow(fnt, entry, 0);

            controller.CallFunction(fnt, new Dictionary<string, dynamic> { { "value", 32.2 } });

            Assert.IsTrue(controller.GetVariableValue(res) == 71.2);

            Assert.IsTrue(controller.GetFunctionParameters(fnt).Count == 1);
            Assert.IsTrue(controller.GetFunctionReturns(fnt).Count == 1);

            // declarators

            controller.ChangeVisibility(ENTITY.CONTEXT_D, 0, "toto", VISIBILITY.PUBLIC);
            controller.ChangeVisibility(ENTITY.VARIABLE, 0, "toto", VISIBILITY.PUBLIC);
            controller.ChangeVisibility(ENTITY.FUNCTION, 0, "toto", VISIBILITY.PUBLIC);
            controller.ChangeVisibility(ENTITY.DATA_TYPE, 0, "toto", VISIBILITY.PUBLIC);
            controller.ChangeVisibility(ENTITY.DATA_TYPE, 0, "tata", VISIBILITY.PUBLIC);
            controller.ChangeVisibility(ENTITY.DATA_TYPE, 0, "tutu", VISIBILITY.PUBLIC);

            uint cnt = controller.Declare(ENTITY.CONTEXT_D, 0, "Container", VISIBILITY.PUBLIC);
            controller.Move(ENTITY.CONTEXT_D, 0, cnt, "toto");
            controller.Move(ENTITY.VARIABLE, 0, cnt, "toto");
            controller.Move(ENTITY.FUNCTION, 0, cnt, "toto");
            controller.Move(ENTITY.DATA_TYPE, 0, cnt, "toto");
            controller.Move(ENTITY.DATA_TYPE, 0, cnt, "tata");
            controller.Move(ENTITY.DATA_TYPE, 0, cnt, "tutu");

            controller.Rename(ENTITY.CONTEXT_D, cnt, "toto", "titi");
            controller.Rename(ENTITY.VARIABLE, cnt, "toto", "titi");
            controller.Rename(ENTITY.FUNCTION, cnt, "toto", "titi");
            controller.Rename(ENTITY.DATA_TYPE, cnt, "toto", "titi");
            controller.Rename(ENTITY.DATA_TYPE, cnt, "tata", "toto");
            controller.Rename(ENTITY.DATA_TYPE, cnt, "tutu", "tata");

            List<Entity> ret = controller.GetEntitiesOfType(ENTITY.CONTEXT_D, cnt);
            variables = controller.GetEntitiesOfType(ENTITY.VARIABLE, cnt);

            Assert.IsTrue(ret.Count == 1);
            Assert.IsTrue(controller.GetEntitiesOfType(ENTITY.VARIABLE, cnt).Count == 1);
            Assert.IsTrue(controller.GetEntitiesOfType(ENTITY.FUNCTION, cnt).Count == 1);
            Assert.IsTrue(controller.GetEntitiesOfType(ENTITY.DATA_TYPE, cnt).Count == 3);

            //controller.Remove(ENTITY.CONTEXT_D, cnt, "titi");
            //controller.Remove(ENTITY.VARIABLE, cnt, "titi");
            //controller.Remove(ENTITY.FUNCTION, cnt, "titi");
            //controller.Remove(ENTITY.DATA_TYPE, cnt, "titi");
            //controller.Remove(ENTITY.DATA_TYPE, cnt, "toto");
            //controller.Remove(ENTITY.DATA_TYPE, cnt, "tata");
        }

        private void GenerateMoreOrLess(ProtobufManager manager, out List<Entity> variables, out List<Entity> functions)
        {
            CoreControl.Controller controller = manager.Controller;
            List<uint> empty = new List<uint>();
            uint integer = (uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE;

            //declaring moreOrLess context in global context
            uint ctx = controller.Declare(ENTITY.CONTEXT_D, 0, "moreOrLess", VISIBILITY.PUBLIC);

            //declaring global variables min, max and lastGiven in moreOrLess context
            uint min = controller.Declare(ENTITY.VARIABLE, ctx, "min", VISIBILITY.PRIVATE);
            controller.SetVariableType(min, integer);
            controller.SetVariableValue(min, 0);
            uint max = controller.Declare(ENTITY.VARIABLE, ctx, "max", VISIBILITY.PRIVATE);
            controller.SetVariableType(max, integer);
            controller.SetVariableValue(max, 100);
            uint lastGiven = controller.Declare(ENTITY.VARIABLE, ctx, "lastGiven", VISIBILITY.PRIVATE);
            controller.SetVariableType(lastGiven, integer);
            controller.SetVariableValue(lastGiven, -1);

            //declaring enumeration COMPARISON in moreOrLess context
            uint COMPARISON = controller.Declare(ENTITY.ENUM_TYPE, ctx, "COMPARISON", VISIBILITY.PUBLIC);
            controller.SetEnumerationValue(COMPARISON, "MORE", 0);
            controller.SetEnumerationValue(COMPARISON, "LESS", 1);
            controller.SetEnumerationValue(COMPARISON, "NONE", 2);

            //declaring function play in moreOrLess context
            uint play = controller.Declare(ENTITY.FUNCTION, ctx, "Play", VISIBILITY.PUBLIC);

            //declaring parameter lastResult in play function
            uint play_lastResult = controller.Declare(ENTITY.VARIABLE, play, "lastResult", VISIBILITY.PUBLIC);
            controller.SetFunctionParameter(play, "lastResult");
            controller.SetVariableType(play_lastResult, COMPARISON);
            controller.SetVariableValue(play_lastResult, controller.GetEnumerationValue(COMPARISON, "NONE"));

            //declaring return result in play function
            uint play_result = controller.Declare(ENTITY.VARIABLE, play, "result", VISIBILITY.PUBLIC);
            controller.SetFunctionReturn(play, "result");
            controller.SetVariableType(play_result, integer);

            uint split_COMPARISON = controller.AddInstruction(play, INSTRUCTION_ID.ENUM_SPLITTER, new List<uint> { COMPARISON });
            uint get_last_result = controller.AddInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { play_lastResult });

            //if (lastResult == COMPARISION::MORE)
            uint lr_eq_more = controller.AddInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.LinkInstructionData(play, split_COMPARISON, "MORE", lr_eq_more, "LeftOperand");
            controller.LinkInstructionData(play, get_last_result, "reference", lr_eq_more, "RightOperand");
            uint if_lr_eq_more = controller.AddInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.LinkInstructionData(play, lr_eq_more, "result", if_lr_eq_more, "condition");

            //min = lastGiven
            uint get_lastGiven = controller.AddInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { lastGiven });
            uint set_min = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { min });
            controller.LinkInstructionData(play, get_lastGiven, "reference", set_min, "value");

            //if (lastResult == COMPARISON::LESS)
            uint lr_eq_less = controller.AddInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.LinkInstructionData(play, get_last_result, "reference", lr_eq_less, "LeftOperand");
            controller.LinkInstructionData(play, split_COMPARISON, "LESS", lr_eq_less, "RightOperand");
            uint if_lr_eq_less = controller.AddInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.LinkInstructionData(play, lr_eq_less, "result", if_lr_eq_less, "condition");

            //max = lastGiven
            uint set_max = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { max });
            controller.LinkInstructionData(play, get_lastGiven, "reference", set_max, "value");

            //min / 2
            uint get_min = controller.AddInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { min });
            uint min_half = controller.AddInstruction(play, INSTRUCTION_ID.DIV, new List<uint> { integer, integer, integer });
            controller.LinkInstructionData(play, get_min, "reference", min_half, "LeftOperand");
            controller.SetInstructionInputValue(play, min_half, "RightOperand", 2);

            //max / 2
            uint get_max = controller.AddInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { max });
            uint max_half = controller.AddInstruction(play, INSTRUCTION_ID.DIV, new List<uint> { integer, integer, integer });
            controller.LinkInstructionData(play, get_max, "reference", max_half, "LeftOperand");
            controller.SetInstructionInputValue(play, max_half, "RightOperand", 2);

            //min / 2 + max / 2
            uint min_half_plus_max_half = controller.AddInstruction(play, INSTRUCTION_ID.ADD, new List<uint> { integer, integer, integer });
            controller.LinkInstructionData(play, min_half, "result", min_half_plus_max_half, "LeftOperand");
            controller.LinkInstructionData(play, max_half, "result", min_half_plus_max_half, "RightOperand");

            //result = min / 2 + max / 2
            uint result_calculation = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.LinkInstructionData(play, min_half_plus_max_half, "result", result_calculation, "value");

            //result == lastGiven
            uint get_result = controller.AddInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { play_result });
            uint res_eq_last_given = controller.AddInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { integer, integer });
            controller.LinkInstructionData(play, get_lastGiven, "reference", res_eq_last_given, "LeftOperand");
            controller.LinkInstructionData(play, get_result, "reference", res_eq_last_given, "RightOperand");

            //if (result == lastGiven)
            uint if_res_eq_last_given = controller.AddInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.LinkInstructionData(play, res_eq_last_given, "result", if_res_eq_last_given, "condition");

            //lastResult == MORE
            uint last_result_eq_more = controller.AddInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.LinkInstructionData(play, get_last_result, "reference", last_result_eq_more, "LeftOperand");
            controller.LinkInstructionData(play, split_COMPARISON, "MORE", last_result_eq_more, "RightOperand");

            //if (lastResult == MORE)
            uint if_last_result_eq_more = controller.AddInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.LinkInstructionData(play, last_result_eq_more, "result", if_last_result_eq_more, "condition");

            //result + 1
            uint result_pp = controller.AddInstruction(play, INSTRUCTION_ID.ADD, new List<uint> { integer, integer, integer });
            controller.LinkInstructionData(play, get_result, "reference", result_pp, "LeftOperand");
            controller.SetInstructionInputValue(play, result_pp, "RightOperand", 1);

            //result = result + 1
            uint set_result_pp = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.LinkInstructionData(play, result_pp, "result", set_result_pp, "value");

            //result - 1
            uint result_mm = controller.AddInstruction(play, INSTRUCTION_ID.SUB, new List<uint> { integer, integer, integer });
            controller.LinkInstructionData(play, get_result, "reference", result_mm, "LeftOperand");
            controller.SetInstructionInputValue(play, result_mm, "RightOperand", 1);

            //result = result - 1
            uint set_result_mm = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.LinkInstructionData(play, result_mm, "result", set_result_mm, "value");

            //lastGiven = result
            uint set_last_given = controller.AddInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { lastGiven });
            controller.LinkInstructionData(play, get_result, "reference", set_last_given, "value");

            /*
             * if (lastResult == More)
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
             *          number = number + 1;
             *      }
             *      else
             *      {
             *          number = number - 1;
             *      }
             * }
             *
             * lastGiven = number
             */
            controller.LinkInstructionExecution(play, if_lr_eq_more, (uint)If.ConditionIndexes.OnTrue, set_min);
            controller.LinkInstructionExecution(play, if_lr_eq_more, (uint)If.ConditionIndexes.OnFalse, if_lr_eq_less);

            controller.LinkInstructionExecution(play, set_min, 0, if_lr_eq_less);

            controller.LinkInstructionExecution(play, if_lr_eq_less, (uint)If.ConditionIndexes.OnTrue, set_max);
            controller.LinkInstructionExecution(play, if_lr_eq_less, (uint)If.ConditionIndexes.OnFalse, result_calculation);

            controller.LinkInstructionExecution(play, set_max, 0, result_calculation);

            controller.LinkInstructionExecution(play, result_calculation, 0, if_res_eq_last_given);

            controller.LinkInstructionExecution(play, if_res_eq_last_given, (uint)If.ConditionIndexes.OnTrue, if_last_result_eq_more);
            controller.LinkInstructionExecution(play, if_res_eq_last_given, (uint)If.ConditionIndexes.OnFalse, set_last_given);

            controller.LinkInstructionExecution(play, if_last_result_eq_more, (uint)If.ConditionIndexes.OnTrue, set_result_pp);
            controller.LinkInstructionExecution(play, if_last_result_eq_more, (uint)If.ConditionIndexes.OnFalse, set_result_mm);

            controller.LinkInstructionExecution(play, set_result_pp, 0, set_last_given);
            controller.LinkInstructionExecution(play, set_result_mm, 0, set_last_given);

            controller.SetFunctionEntryPoint(play, if_lr_eq_more);

            //int mystery_number = 47;

            //int i = 0;

            //Dictionary<string, dynamic> args = new Dictionary<string, dynamic>
            //{
            //    { "lastResult", controller.GetEnumerationValue(COMPARISON, "NONE") }
            //};

            //Dictionary<string, dynamic> returns;

            //do
            //{
            //    returns = controller.CallFunction(play, args);

            //    string toprint = "IA give: " + returns["result"].ToString();

            //    System.Diagnostics.Debug.WriteLine(toprint);

            //    if (returns["result"] > mystery_number)
            //    {
            //        args["lastResult"] = controller.GetEnumerationValue(COMPARISON, "LESS");
            //        System.Diagnostics.Debug.WriteLine("==> It's less");
            //    }
            //    else if (returns["result"] < mystery_number)
            //    {
            //        args["lastResult"] = controller.GetEnumerationValue(COMPARISON, "MORE");
            //        System.Diagnostics.Debug.WriteLine("==> It's more");
            //    }
            //    else
            //        break;
            //    ++i;
            //} while (returns["result"] != mystery_number && i < 10);

            //if (i == 10)
            //    throw new Exception("Failed to reach mystery number in less that 10 times");
            //else
            //    System.Diagnostics.Debug.Write("AI found the mystery number: " + mystery_number.ToString());

            variables = controller.GetEntitiesOfType(ENTITY.VARIABLE, 0);
            functions = controller.GetEntitiesOfType(ENTITY.FUNCTION, 0);
        }
    }
}