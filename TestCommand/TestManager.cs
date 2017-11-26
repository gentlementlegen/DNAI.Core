using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
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

            dispatcher.SaveCommandsTo("test.duly");
            dispatcher.LoadCommandsFrom("test.duly");
        }
    }
}
