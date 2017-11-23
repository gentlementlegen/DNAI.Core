using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestCommand
{
    [TestClass]
    public class TestManager
    {
        [TestMethod]
        public void ManagerCoverage()
        {
            CoreCommand.IManager dispatcher = new CoreCommand.ProtobufManager();

            Stream instream = new MemoryStream();
            Stream outStream = new MemoryStream();

            CoreCommand.Command.Declare toserial = new CoreCommand.Command.Declare
            {
                ContainerID = 0,
                EntityType = CoreControl.EntityFactory.ENTITY.CONTEXT,
                Name = "toto",
                Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
            };

            ProtoBuf.Serializer.SerializeWithLengthPrefix(instream, toserial, ProtoBuf.PrefixStyle.Base128);
            instream.Position = 0;

            dispatcher.onDeclare(instream, outStream);

            outStream.Position = 0;
            CoreCommand.Reply.EntityDeclared declared = ProtoBuf.Serializer.DeserializeWithLengthPrefix<CoreCommand.Reply.EntityDeclared>(outStream, ProtoBuf.PrefixStyle.Base128);

            Assert.IsTrue(
                declared.Command.ContainerID == 0
                && declared.Command.EntityType == CoreControl.EntityFactory.ENTITY.CONTEXT
                && declared.Command.Name == "toto"
                && declared.Command.Visibility == CoreControl.EntityFactory.VISIBILITY.PUBLIC
                && declared.EntityID == 6);
        }
    }
}
