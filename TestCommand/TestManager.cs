using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestCommand
{
    [TestClass]
    public class TestManager
    {
        private void testCommand<Command, Reply>(CoreCommand.IManager manager, Command toserial, Action<Stream, Stream> toCall, Action<Command, Reply> check)
        {
            Stream instream = new MemoryStream();
            Stream outStream = new MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(instream, toserial, ProtoBuf.PrefixStyle.Base128);
            instream.Position = 0;

            toCall(instream, outStream);

            outStream.Position = 0;
            Reply reply = ProtoBuf.Serializer.DeserializeWithLengthPrefix<Reply>(outStream, ProtoBuf.PrefixStyle.Base128);

            check(toserial, reply);
        }

        //[TestMethod]
        public void ManagerCoverage()
        {
            CoreCommand.IManager dispatcher = new CoreCommand.ProtobufManager();

            testCommand(
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

            testCommand(
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

            testCommand(
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

            testCommand(
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

            testCommand(
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
                dispatcher.OnRemove,
                (CoreCommand.Command.Remove message, CoreCommand.Reply.Remove reply) =>
                {
                    Assert.IsTrue(
                        message.Name == reply.Command.Name
                        && message.ContainerID == reply.Command.ContainerID
                        && message.EntityType == reply.Command.EntityType
                        );
                });

            dispatcher.SaveCommandsTo("test.duly");
            dispatcher.LoadCommandsFrom("test.duly");
        }
    }
}