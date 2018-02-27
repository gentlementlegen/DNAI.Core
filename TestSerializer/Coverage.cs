using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace TestSerializer
{
    [TestClass]
    public class Coverage
    {
        public class Nested
        {
            [BinarySerializer.BinaryFormat]
            public Int32 posX { get; set; }

            [BinarySerializer.BinaryFormat]
            public Int32 posY { get; set; }
        }

        public enum VISI
        {
            PUBLIC = 49,
            PRIVATE = 120
        }

        public class TestSerial
        {
            [BinarySerializer.BinaryFormat]
            public Char tutu { get; set; }

            [BinarySerializer.BinaryFormat]
            public Int16 toto16 { get; set; }

            [BinarySerializer.BinaryFormat]
            public Int32 toto32 { get; set; }

            [BinarySerializer.BinaryFormat]
            public Int64 toto64 { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt16 tutu16 { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt32 tutu32 { get; set; }

            [BinarySerializer.BinaryFormat]
            public UInt64 tutu64 { get; set; }

            [BinarySerializer.BinaryFormat]
            public float tata { get; set; }

            [BinarySerializer.BinaryFormat]
            public Double titi { get; set; }

            [BinarySerializer.BinaryFormat]
            public String toto { get; set; }

            [BinarySerializer.BinaryFormat]
            public List<Double> multi { get; set; }

            [BinarySerializer.BinaryFormat]
            public Nested test { get; set; }

            [BinarySerializer.BinaryFormat]
            public VISI vis { get; set; }
        }

        [TestMethod]
        public void SerializerCoverage()
        {
            TestSerial datatoserial = new TestSerial
            {
                tutu = 'G', //1
                toto16 = -42, //2
                toto32 = -2000000, //4
                toto64 = -5000000000, //8
                tutu16 = 42, //2
                tutu32 = 2000000, //4
                tutu64 = 5000000000, //8
                tata = 3.14f, //4
                titi = 3.14, //8
                toto = "Salut", //???
                multi = new List<double> //28
                {
                    12.3,
                    14.2,
                    -4.93
                },
                test = new Nested //8
                {
                    posX = 493,
                    posY = -394
                },
                vis = VISI.PRIVATE
            };

            FileStream file = File.Create("toto.txt");
            byte[] buff = new byte[4096];

            //Console.WriteLine("Serializing 3: " + BinarySerializer.Serializer.Serialize(3, buff));
            //file.Write(buff, 0, 4);

            int len = BinarySerializer.Serializer.Serialize(datatoserial, buff);
            Console.WriteLine("Serializing obj: " + len);
            file.Write(buff, 0, len);

            file.Close();

            datatoserial = null;

            datatoserial = BinarySerializer.Serializer.Deserialize<TestSerial>(File.ReadAllBytes("toto.txt"));

            Assert.IsTrue(datatoserial.tutu == 'G');
            Assert.IsTrue(datatoserial.toto16 == -42);
            Assert.IsTrue(datatoserial.toto32 == -2000000);
            Assert.IsTrue(datatoserial.toto64 == -5000000000);
            Assert.IsTrue(datatoserial.tutu16 == 42);
            Assert.IsTrue(datatoserial.tutu32 == 2000000);
            Assert.IsTrue(datatoserial.tutu64 == 5000000000);
            Assert.IsTrue(datatoserial.tata == 3.14f);
            Assert.IsTrue(datatoserial.titi == 3.14);
            Assert.IsTrue(datatoserial.toto == "Salut");
            Assert.IsTrue(datatoserial.multi.Count == 3);

            double[] cmp = new double[3] { 12.3, 14.2, -4.93 };
            int i = 0;

            foreach (double item in datatoserial.multi)
            {
                Assert.IsTrue(item == cmp[i]);
                i++;
            }

            Assert.IsTrue(datatoserial.test.posX == 493);
            Assert.IsTrue(datatoserial.test.posY == -394);

            Assert.IsTrue(datatoserial.vis == VISI.PRIVATE);
        }
    }
}
