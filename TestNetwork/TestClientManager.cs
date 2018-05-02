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
            Exception err = null;
            Process server = null;
            String serverDirectory = Directory.GetCurrentDirectory() + "\\..\\..\\Server";
            String serverZip = serverDirectory + ".zip";

            try
            {
                CoreNetwork.ClientManager coreSide = new CoreNetwork.ClientManager(new CoreCommand.BinaryManager());
                EventServerClient.Communication.TcpManager guiSide = new EventServerClient.Communication.TcpManager();

                Assert.IsTrue(File.Exists(serverZip));

                if (Directory.Exists(serverDirectory))
                    Directory.Delete(serverDirectory, true);

                ZipFile.ExtractToDirectory(serverZip, serverDirectory);

                String serverExe = serverDirectory + "\\Server.exe";

                Assert.IsTrue(File.Exists(serverExe));

                server = Process.Start(serverExe, "-p 4242");

                Thread.Sleep(500);

                coreSide.Connect("127.0.0.1", 4242);
                guiSide.Connect("127.0.0.1", 4242);

                Assert.IsTrue(coreSide.isConnected() && guiSide.isConnected());

                coreSide.RegisterEvents();

                CoreCommand.Command.Declarator.Declare tosend = new CoreCommand.Command.Declarator.Declare
                {
                    ContainerID = 0,
                    EntityType = CoreControl.EntityFactory.ENTITY.CONTEXT,
                    Name = "testCoverage",
                    Visibility = CoreControl.EntityFactory.VISIBILITY.PUBLIC
                };

                bool run = true;

                guiSide.RegisterEvent("DECLARATOR.DECLARED", (byte[] data) =>
                {
                    MemoryStream stream = new MemoryStream(data);

                    CoreCommand.Command.Declarator.Declare.Reply reply = BinarySerializer.Serializer.Deserialize<CoreCommand.Command.Declarator.Declare.Reply>(data);

                    Debug.WriteLine("Reply: { {" + tosend.ContainerID + ", " + tosend.EntityType + ", \"" + tosend.Name + "\", " + tosend.Visibility + "}, " + reply.EntityID + "}");

                    run = false;
                }, 0);

                MemoryStream sendstream = new MemoryStream();

                BinarySerializer.Serializer.Serialize(tosend, sendstream);

                guiSide.SendEvent("DECLARATOR.DECLARE", sendstream.GetBuffer());

                uint timeout = 100;

                while (run && timeout > 0)
                {
                    coreSide.Update();
                    guiSide.Update();
                    Thread.Sleep(50);
                    --timeout;
                }

                Assert.IsFalse(run);
            }
            catch (Exception error)
            {
                err = error;
                Console.Error.WriteLine(error.Message);
            }
            
            if (server != null)
            {
                server.Kill();
                server.WaitForExit();
            }
            
            Directory.Delete(serverDirectory, true);

            if (err != null)
                throw err;
        }
    }
}
