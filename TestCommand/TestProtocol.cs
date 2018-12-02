using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCommand
{
    [TestClass]
    public class TestProtocol
    {
        [TestMethod]
        public void TestBinary()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.Global.SetProtocol
            {
                Protocol = CoreCommand.Command.Global.SetProtocol.ProtocolValues.BINARY
            }));

            MemoryStream input = new MemoryStream();

            BinarySerializer.Serializer.Serialize(new CoreCommand.Command.Global.Load
            {
                Filename = "astar.dnai"
            }, input);
            input.Position = 0;

            MemoryStream output = new MemoryStream();

            Assert.IsTrue(manager.CallCommand("GLOBAL.LOAD_FROM", input, output));
        }

        [TestMethod]
        public void TestJSON()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            Assert.IsTrue(manager.CallCommand(new CoreCommand.Command.Global.SetProtocol
            {
                Protocol = CoreCommand.Command.Global.SetProtocol.ProtocolValues.JSON
            }));

            MemoryStream input = new MemoryStream();

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(new CoreCommand.Command.Global.Load
            {
                Filename = "astar.dnai"
            });
            byte[] bdata = Encoding.UTF8.GetBytes(data);
            input.Write(bdata, 0, bdata.Length);
            input.Position = 0;

            MemoryStream output = new MemoryStream();

            Assert.IsTrue(manager.CallCommand("GLOBAL.LOAD_FROM", input, output));
        }
    }
}
