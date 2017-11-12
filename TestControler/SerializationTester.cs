using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestControler
{
    [TestClass]
    public class SerializationTester
    {
        [TestMethod]
        public void SerializeParameters()
        {
            Console.Write(ProtoBuf.Serializer.GetProto<CoreControl.Command.Default>(ProtoBuf.Meta.ProtoSyntax.Proto3));
        }
    }
}
