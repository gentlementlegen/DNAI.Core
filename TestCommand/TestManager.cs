using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestCommand
{
    [TestClass]
    public class TestManager
    {
        private void testCommand<Command, Reply>(CoreCommand.IManager manager, Command toserial, Func<Stream, Stream, bool> toCall, Action<Command, Reply> check)
        {
            Stream instream = new MemoryStream();
            Stream outStream = new MemoryStream();

            BinarySerializer.Serializer.Serialize(toserial, instream);
            //ProtoBuf.Serializer.SerializeWithLengthPrefix(instream, toserial, ProtoBuf.PrefixStyle.Base128);
            instream.Position = 0;

            Assert.IsTrue(toCall(instream, outStream));

            outStream.Position = 0;
            Reply reply = BinarySerializer.Serializer.Deserialize<Reply>(outStream);
            //ProtoBuf.Serializer.DeserializeWithLengthPrefix<Reply>(outStream, ProtoBuf.PrefixStyle.Base128);

            check(toserial, reply);
        }

        [TestMethod]
        public void ManagerCoverage()
        {
            CoreCommand.IManager dispatcher = new CoreCommand.BinaryManager();

            testCommand(
                dispatcher,
                new CoreCommand.Command.Declare
                {
                    ContainerID = 0,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "toto",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.GetCommand("DECLARE"),
                (CoreCommand.Command.Declare command, CoreCommand.Command.Declare.Reply reply) =>
                {
                    Assert.IsTrue(
                       reply.Command.ContainerID == command.ContainerID
                       && reply.Command.EntityType == command.EntityType
                       && reply.Command.Name == command.Name
                       && reply.Command.Visibility == command.Visibility
                       && reply.EntityID == 6);
                });

            testCommand(
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

            testCommand(
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

            testCommand(
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

            testCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 6,
                    TypeID = 2
                },
                dispatcher.GetCommand("SET_VARIABLE_TYPE"),
                (CoreCommand.Command.SetVariableType message, CoreCommand.Command.SetVariableType.Reply reply) =>
                {
                    Assert.IsTrue(
                        reply.Command.VariableID == message.VariableID
                        && reply.Command.TypeID == message.TypeID);
                });

            testCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 6,
                    Value = "42"
                },
                dispatcher.GetCommand("SET_VARIABLE_VALUE"),
                (CoreCommand.Command.SetVariableValue message, CoreCommand.Command.SetVariableValue.Reply reply) =>
                {
                    Assert.IsTrue(
                        message.VariableID == reply.Command.VariableID
                        && message.Value == reply.Command.Value
                        );
                });

            testCommand(
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

            testCommand(
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

            testCommand(
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

            testCommand(
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

            testCommand(
                dispatcher,
                new CoreCommand.Command.ChangeVisibility
                {
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    ContainerID = 0,
                    Name = "toto",
                    NewVisi = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.GetCommand("CHANGE_VISIBILITY"),
                (CoreCommand.Command.ChangeVisibility message, CoreCommand.Command.ChangeVisibility.Reply reply) =>
                {
                    Assert.IsTrue(
                        message.Name == reply.Command.Name
                        && message.ContainerID == reply.Command.ContainerID
                        && message.EntityType == reply.Command.EntityType
                        && message.NewVisi == reply.Command.NewVisi
                        );
                });

            testCommand(
                dispatcher,
                new CoreCommand.Command.GetVariableValue
                {
                    VariableId = 6
                },
                dispatcher.GetCommand("GET_VARIABLE_VALUE"),
                (CoreCommand.Command.GetVariableValue message, CoreCommand.Command.GetVariableValue.Reply reply) =>
                {
                    Assert.IsTrue(
                        message.VariableId == reply.Command.VariableId
                        );
                });
            //testCommand(
            //    dispatcher,
            //    new CoreCommand.Command.Move
            //    {
            //        EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
            //        Name = "toto",
            //        FromID = 6,
            //        ToID = 6
            //    },
            //    dispatcher.OnMove,
            //    (CoreCommand.Command.Move message, CoreCommand.Reply.Move reply) =>
            //    {
            //        Assert.IsTrue(
            //            message.Name == reply.Command.Name
            //            && message.FromID == reply.Command.FromID
            //            && message.EntityType == reply.Command.EntityType
            //            && message.ToID == reply.Command.ToID
            //            );
            //    });

            testCommand(
                dispatcher,
                new CoreCommand.Command.Remove
                {
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    ContainerID = 0,
                    Name = "toto"
                },
                dispatcher.GetCommand("REMOVE"),
                (CoreCommand.Command.Remove message, CoreCommand.Command.Remove.Reply reply) =>
                {
                    Assert.IsTrue(
                        message.Name == reply.Command.Name
                        && message.ContainerID == reply.Command.ContainerID
                        && message.EntityType == reply.Command.EntityType
                        );
                });

            testCommand(
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

            testCommand(
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

            testCommand(
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
            dispatcher.LoadCommandsFrom("test.duly");
        }
    }
}