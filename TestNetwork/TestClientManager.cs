using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace TestNetwork
{
    [TestClass]
    public class TestClientManager
    {
        [TestMethod]
        public void ClientManagerCoverage()
        {
            CoreNetwork.ClientManager coreSide = new CoreNetwork.ClientManager(new CoreCommand.ProtobufManager());
            EventServerClient.Communication.TcpManager guiSide = new EventServerClient.Communication.TcpManager();

            coreSide.Connect("127.0.0.1", 8765);
            guiSide.Connect("127.0.0.1", 8765);

            Assert.IsTrue(coreSide.isConnected() && guiSide.isConnected());

            coreSide.RegisterEvents();

            CoreCommand.Command.Declare tosend = new CoreCommand.Command.Declare {
                ContainerID = 0,
                EntityType = CoreControl.EntityFactory.ENTITY.CONTEXT_D,
                Name = "testCoverage",
                Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
            };

            bool run = true;

            guiSide.RegisterEvent("ENTITY_DECLARED", (byte[] data) =>
            {
                MemoryStream stream = new MemoryStream(data);

                CoreCommand.Reply.EntityDeclared reply = ProtoBuf.Serializer.Deserialize<CoreCommand.Reply.EntityDeclared>(stream);

                Debug.WriteLine("Reply: { {" + reply.Command.ContainerID + ", " + reply.Command.EntityType + ", \"" + reply.Command.Name + "\", " + reply.Command.Visibility + "}, " + reply.EntityID + "}");
                Assert.IsTrue(
                    tosend.ContainerID == reply.Command.ContainerID
                    && tosend.EntityType == reply.Command.EntityType
                    && tosend.Name == reply.Command.Name
                    && tosend.Visibility == reply.Command.Visibility);

                run = false;
            }, 0);

            MemoryStream sendstream = new MemoryStream();

            ProtoBuf.Serializer.Serialize(sendstream, tosend);

            guiSide.SendEvent("DECLARE", sendstream.GetBuffer());

            while (run)
            {
                coreSide.Update();
                guiSide.Update();
                Thread.Sleep(50);
            }
        }
    }
}
