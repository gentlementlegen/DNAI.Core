using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Security.AccessControl;

namespace TestNetwork
{
    [TestClass]
    public class TestClientManager
    {
        [TestMethod]
        public void ClientManagerCoverage()
        {
            CoreNetwork.ClientManager coreSide = new CoreNetwork.ClientManager(new CoreCommand.BinaryManager());
            EventServerClient.Communication.TcpManager guiSide = new EventServerClient.Communication.TcpManager();

            String serverDirectory = Directory.GetCurrentDirectory() + "\\..\\..\\Server";
            String serverZip = serverDirectory + ".zip";

            Assert.IsTrue(File.Exists(serverZip));

            if (Directory.Exists(serverDirectory))
                Directory.Delete(serverDirectory, true);

            ZipFile.ExtractToDirectory(serverZip, serverDirectory);

            String serverExe = serverDirectory + "\\Server.exe";

            Assert.IsTrue(File.Exists(serverExe));

            Process server = Process.Start(serverExe, "-p 4242");

            Thread.Sleep(500);

            coreSide.Connect("127.0.0.1", 4242);
            guiSide.Connect("127.0.0.1", 4242);

            Assert.IsTrue(coreSide.isConnected() && guiSide.isConnected());

            coreSide.RegisterEvents();

            CoreCommand.Command.Declare tosend = new CoreCommand.Command.Declare {
                ContainerID = 0,
                EntityType = CoreControl.EntityFactory.ENTITY.CONTEXT,
                Name = "testCoverage",
                Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
            };

            bool run = true;

            guiSide.RegisterEvent("ENTITY_DECLARED", (byte[] data) =>
            {
                MemoryStream stream = new MemoryStream(data);

                CoreCommand.Command.Declare.Reply reply = BinarySerializer.Serializer.Deserialize<CoreCommand.Command.Declare.Reply>(data);//ProtoBuf.Serializer.DeserializeWithLengthPrefix<CoreCommand.Reply.EntityDeclared>(stream, ProtoBuf.PrefixStyle.Base128);

                Debug.WriteLine("Reply: { {" + reply.Command.ContainerID + ", " + reply.Command.EntityType + ", \"" + reply.Command.Name + "\", " + reply.Command.Visibility + "}, " + reply.EntityID + "}");
                Assert.IsTrue(
                    tosend.ContainerID == reply.Command.ContainerID
                    && tosend.EntityType == reply.Command.EntityType
                    && tosend.Name == reply.Command.Name
                    && tosend.Visibility == reply.Command.Visibility);

                run = false;
            }, 0);

            MemoryStream sendstream = new MemoryStream();

            BinarySerializer.Serializer.Serialize(tosend, sendstream);
            //ProtoBuf.Serializer.SerializeWithLengthPrefix(sendstream, tosend, ProtoBuf.PrefixStyle.Base128);

            guiSide.SendEvent("DECLARE", sendstream.GetBuffer());

            while (run)
            {
                coreSide.Update();
                guiSide.Update();
                Thread.Sleep(50);
            }

            server.Kill();

            Directory.Delete(serverDirectory, true);
        }
    }
}
