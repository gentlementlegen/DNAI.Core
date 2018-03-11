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
                dispatcher.GetCommand("DECLARATOR.DECLARE"),
                (CoreCommand.Command.Declare command, CoreCommand.Command.Declare.Reply reply) =>
                {
                    Assert.IsTrue(reply.EntityID == 6);
                });

            testCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableType
                {
                    VariableID = 6,
                    TypeID = 2
                },
                dispatcher.GetCommand("VARIABLE.SET_TYPE"),
                (CoreCommand.Command.SetVariableType message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.SetVariableValue
                {
                    VariableID = 6,
                    Value = "42"
                },
                dispatcher.GetCommand("VARIABLE.SET_VALUE"),
                (CoreCommand.Command.SetVariableValue message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.ChangeVisibility
                {
                    ContainerID = 0,
                    Name = "toto",
                    NewVisi = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                dispatcher.GetCommand("DECLARATOR.CHANGE_VISIBILITY"),
                (CoreCommand.Command.ChangeVisibility message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.GetVariableValue
                {
                    VariableId = 6
                },
                dispatcher.GetCommand("VARIABLE.GET_VALUE"),
                (CoreCommand.Command.GetVariableValue message, CoreCommand.Command.GetVariableValue.Reply reply) =>
                {
                    Assert.IsTrue(reply.Value == "42");
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
                    ContainerID = 0,
                    Name = "toto"
                },
                dispatcher.GetCommand("DECLARATOR.REMOVE"),
                (CoreCommand.Command.Remove message, CoreCommand.EmptyReply reply) => { });

            dispatcher.SaveCommandsTo("test.duly");
            dispatcher.LoadCommandsFrom("test.duly");
        }
    }
}