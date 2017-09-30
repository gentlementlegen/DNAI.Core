using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Communication;

namespace CoreTestNet
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TcpManager tcp = new TcpManager(4242);
            tcp.StartListening();
        }
    }
}
