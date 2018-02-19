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

            CorePackage.Entity.Function getAttrSum = new CorePackage.Entity.Function();
            ((CorePackage.Global.IDeclarator<CorePackage.Entity.Function>)type).Declare(getAttrSum, "getAttrSum", CorePackage.Global.AccessMode.EXTERNAL);
            type.SetFunctionAsMember("getAttrSum", CorePackage.Global.AccessMode.EXTERNAL);

            CorePackage.Entity.Variable res = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer);
            getAttrSum.Declare(res, "res", CorePackage.Global.AccessMode.EXTERNAL);
            getAttrSum.SetVariableAs("res", CorePackage.Entity.Function.VariableRole.RETURN);

            Dictionary<string, dynamic> t = type.Instantiate();

            Assert.IsTrue(t.ContainsKey("x") && t.ContainsKey("y") && t.ContainsKey("z"));

            CorePackage.Entity.Variable tvar = new CorePackage.Entity.Variable(type);
            
            tvar.Value["x"] = 3;
            tvar.Value["y"] = 42;
            tvar.Value["z"] = -29;

            //show fields

            uint getT = getAttrSum.addInstruction(new CorePackage.Execution.Getter(tvar));

            uint getAttrs = getAttrSum.addInstruction(new CorePackage.Execution.ObjectAttributes(type));
            getAttrSum.LinkInstructionData(getT, "reference", getAttrs, "this");

            uint setRes = getAttrSum.addInstruction(new CorePackage.Execution.Setter(res));

            //x + y
            uint xPy = getAttrSum.addInstruction(new CorePackage.Execution.Operators.Add(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer));
            getAttrSum.LinkInstructionData(getAttrs, "x", xPy, CorePackage.Global.Operator.Left);
            getAttrSum.LinkInstructionData(getAttrs, "y", xPy, CorePackage.Global.Operator.Right);

            //x + y + z
            uint xPyPz = getAttrSum.addInstruction(new CorePackage.Execution.Operators.Add(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer));
            getAttrSum.LinkInstructionData(xPy, CorePackage.Global.Operator.Result, xPyPz, CorePackage.Global.Operator.Left);
            getAttrSum.LinkInstructionData(getAttrs, "z", xPyPz, CorePackage.Global.Operator.Right);

            getAttrSum.LinkInstructionData(xPyPz, CorePackage.Global.Operator.Result, setRes, "value");

            getAttrSum.setEntryPoint(setRes);

            //System.IO.File.WriteAllText("toto.dot", getAttrSum.ToDotFile());
            getAttrSum.Call();

            Console.WriteLine(getAttrSum.GetReturnValue("res"));
            Assert.IsTrue(getAttrSum.GetReturnValue("res") == 16);
            //method call -> nouvel objet method qui hérite de function -> rename de l'instruction FunctionCall en Call
        }
    }
}
