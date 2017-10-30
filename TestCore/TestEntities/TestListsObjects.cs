using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTest.TestEntities
{
    [TestClass]
    public class TestListsObjects
    {
        [TestMethod]
        public void TestListActions()
        {
            CorePackage.Entity.DataType type = new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Integer);

            var t = type.Instantiate();

            Assert.IsTrue(type.IsValid());
            //Assert.IsTrue(type.IsValueOfType(12));

            //var lol = CorePackage.Entity.Type.Scalar.Integer;
            //lol.IsValid();
            //Assert.IsTrue(lol.IsValueOfType(12));
            //Assert.IsTrue(lol.IsValueOfType(typeof(CorePackage.Entity.Type.Scalar)));

            //append
            //  input:
            //      - value
            t.Add(CorePackage.Entity.Type.Scalar.Integer.Instantiate());

            //foreach
            //  outputs:
            //      - index
            //      - value
            for (int i = 0; i < t.Count; i++)
            {
                Debug.WriteLine($"Index {i} ; value = {t[i]}");
            }

            //insert
            //  inputs:
            //      - index
            //      - value
            t.Insert(0, CorePackage.Entity.Type.Scalar.Integer.Instantiate());

            //remove
            //  inputs:
            //      - index
            t.RemoveAt(1);

            //clear
            t.Clear();

            //size
            //  outputs:
            //      - size
            Debug.WriteLine($"Size = {t.Count}");
        }

        [TestMethod]
        public void TestObjectActions()
        {
            CorePackage.Entity.Type.ObjectType type = new CorePackage.Entity.Type.ObjectType(null);

            type.AddAttribute("x", CorePackage.Entity.Type.Scalar.Integer, CorePackage.Global.AccessMode.EXTERNAL);
            type.AddAttribute("y", CorePackage.Entity.Type.Scalar.Integer, CorePackage.Global.AccessMode.EXTERNAL);
            type.AddAttribute("z", CorePackage.Entity.Type.Scalar.Integer, CorePackage.Global.AccessMode.EXTERNAL);
            
            //show fields

            //method call -> nouvel objet method qui hérite de function -> rename de l'instruction FunctionCall en Call
        }
    }
}
