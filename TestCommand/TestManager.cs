using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestCommand
{
    [TestClass]
    public class TestManager
    {
        private void testCommand<Command, Reply>(CoreCommand.IManager manager, Command toserial, String toCall, Action<Command, Reply> check)
        {
            Stream instream = new MemoryStream();
            Stream outStream = new MemoryStream();

            BinarySerializer.Serializer.Serialize(toserial, instream);
            //ProtoBuf.Serializer.SerializeWithLengthPrefix(instream, toserial, ProtoBuf.PrefixStyle.Base128);
            instream.Position = 0;

            Assert.IsTrue(manager.CallCommand(toCall, instream, outStream));

            outStream.Position = 0;
            Command cmd = BinarySerializer.Serializer.Deserialize<Command>(outStream);
            Reply reply = BinarySerializer.Serializer.Deserialize<Reply>(outStream);
            //ProtoBuf.Serializer.DeserializeWithLengthPrefix<Reply>(outStream, ProtoBuf.PrefixStyle.Base128);

            check(toserial, reply);
        }

        //[TestMethod]
        public void ManagerCoverage()
        {
            CoreCommand.IManager dispatcher = new CoreCommand.BinaryManager();

            testCommand(
                dispatcher,
                new CoreCommand.Command.Global.CreateProject
                {
                    ProjectName = "Testor"
                },
                "GLOBAL.CREATE_PROJECT",
                (CoreCommand.Command.Global.CreateProject command, CoreCommand.Command.Global.CreateProject.Reply reply) =>
                {
                    Assert.IsTrue(reply.RootId == 6);
                }
            );

            testCommand(
                dispatcher,
                new CoreCommand.Command.Declarator.Declare
                {
                    ContainerID = 6,
                    EntityType = CoreControl.EntityFactory.ENTITY.VARIABLE,
                    Name = "toto",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                "DECLARATOR.DECLARE",
                (CoreCommand.Command.Declarator.Declare command, CoreCommand.Command.Declarator.Declare.Reply reply) =>
                {
                    Assert.IsTrue(reply.EntityID == 7);
                });

            testCommand(
                dispatcher,
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = 7,
                    TypeID = 2
                },
                "VARIABLE.SET_TYPE",
                (CoreCommand.Command.Variable.SetType message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = 7,
                    Value = "42"
                },
                "VARIABLE.SET_VALUE",
                (CoreCommand.Command.Variable.SetValue message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.Declarator.SetVisibility
                {
                    ContainerID = 6,
                    Name = "toto",
                    NewVisi = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                },
                "DECLARATOR.SET_VISIBILITY",
                (CoreCommand.Command.Declarator.SetVisibility message, CoreCommand.EmptyReply reply) => { });

            testCommand(
                dispatcher,
                new CoreCommand.Command.Variable.GetValue
                {
                    VariableId = 7
                },
                "VARIABLE.GET_VALUE",
                (CoreCommand.Command.Variable.GetValue message, CoreCommand.Command.Variable.GetValue.Reply reply) =>
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
                new CoreCommand.Command.Declarator.Remove
                {
                    ContainerID = 6,
                    Name = "toto"
                },
                "DECLARATOR.REMOVE",
                (CoreCommand.Command.Declarator.Remove message, CoreCommand.EmptyReply reply) => { });

            dispatcher.SaveCommandsTo("test.duly");
            dispatcher.Reset();
            dispatcher.LoadCommandsFrom("test.duly");
            
            testCommand(
                dispatcher,
                new CoreCommand.Command.Global.RemoveProject
                {
                    ProjectName = "Testor"
                },
                "GLOBAL.REMOVE_PROJECT",
                (CoreCommand.Command.Global.RemoveProject cmd, CoreCommand.Command.Global.RemoveProject.Reply reply) =>
                {
                    Assert.IsTrue(reply.Removed.Count == 1);
                });
        }
    }
}