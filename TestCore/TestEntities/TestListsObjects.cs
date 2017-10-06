using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            CorePackage.Entity.Type.ListType type = new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Integer);

            //append
            //  input:
            //      - value

            //foreach
            //  outputs:
            //      - index
            //      - value

            //insert
            //  inputs:
            //      - index
            //      - value

            //remove
            //  inputs:
            //      - index

            //clear
            
            //size
            //  outputs:
            //      - size
        }

        [TestMethod]
        public void TestObjectActions()
        {
            CorePackage.Entity.Type.ObjectType type = new CorePackage.Entity.Type.ObjectType(null);

            type.AddPublicAttribute("x", CorePackage.Entity.Type.Scalar.Integer);
            type.AddPublicAttribute("y", CorePackage.Entity.Type.Scalar.Integer);
            type.AddPublicAttribute("z", CorePackage.Entity.Type.Scalar.Integer);
            
            //show fields

            //method call -> nouvel objet method qui hérite de function -> rename de l'instruction FunctionCall en Call
        }
    }
}
