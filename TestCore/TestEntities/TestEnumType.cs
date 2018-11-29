using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.TestEntities
{
    [TestClass]
    public class TestEnumType
    {
        public enum COMPARISON
        {
            NONE = 0,
            MORE,
            LESS
        }

        [TestMethod]
        public void TestJsonValues()
        {
            var enumType = new CorePackage.Entity.Type.EnumType();

            enumType.Name = "MoreOrLess";
            enumType.SetValue("NONE", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 0));
            enumType.SetValue("MORE", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 1));
            enumType.SetValue("LESS", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 2));
            
            Assert.IsTrue(enumType.CreateFromJSON("MoreOrLess.NONE") == 0);

            Assert.IsTrue(enumType.CreateFromJSON("2") == 2);
        }

        [TestMethod]
        public void TestTypeCompatibility()
        {
            var enumType = new CorePackage.Entity.Type.EnumType();

            enumType.SetValue("NONE", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 0));
            enumType.SetValue("MORE", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 1));
            enumType.SetValue("LESS", new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 2));

            short shortVal = 0;
            ushort ushortVal = 0;
            int intVal = 1;
            uint uintVal = 1;
            long longVal = 2;
            long ulongVal = 2;
            COMPARISON enumVal = COMPARISON.MORE;

            Assert.IsTrue(enumType.IsValueOfType(shortVal));
            Assert.IsTrue(enumType.IsValueOfType(ushortVal));
            Assert.IsTrue(enumType.IsValueOfType(intVal));
            Assert.IsTrue(enumType.IsValueOfType(uintVal));
            Assert.IsTrue(enumType.IsValueOfType(longVal));
            Assert.IsTrue(enumType.IsValueOfType(ulongVal));
            Assert.IsTrue(enumType.IsValueOfType(enumVal));
        }
    }
}
