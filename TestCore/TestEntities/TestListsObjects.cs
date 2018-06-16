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
            CorePackage.Entity.Type.ScalarType integer = CorePackage.Entity.Type.Scalar.Integer;
            CorePackage.Entity.DataType type = new CorePackage.Entity.Type.ListType(integer);

            var t = type.Instantiate();

            Assert.IsTrue(type.IsValid());

            //append
            //  input:
            //      - value
            t.Add(integer.Instantiate());

            Assert.IsTrue(type.OperatorEqual(t, new List<int> { 0 }));

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

            Assert.IsTrue(type.OperatorEqual(t, new List<int> { 0, 0 }));

            var operatorTest = type.Instantiate();

            operatorTest.Add(42);
            operatorTest.Add(50);

            var union = type.OperatorAdd(t, operatorTest);

            t.Add(42);

            var sub = type.OperatorSub(operatorTest, t);

            t.Add(50);

            var item = type.OperatorAccess(operatorTest, 1);

            Assert.IsTrue(type.OperatorEqual(union, new List<long> { 0, 0, 42, 50 }));
            Assert.IsTrue(type.OperatorEqual(sub, new List<long> { 50 }));
            Assert.IsTrue(item == 50);

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
            CorePackage.Entity.Type.ObjectType type = new CorePackage.Entity.Type.ObjectType();
            CorePackage.Entity.DataType integer = CorePackage.Entity.Type.Scalar.Integer;

            type.AddAttribute("x", integer, CorePackage.Global.AccessMode.EXTERNAL);
            type.AddAttribute("y", integer, CorePackage.Global.AccessMode.EXTERNAL);
            type.AddAttribute("z", integer, CorePackage.Global.AccessMode.EXTERNAL);
            
            CorePackage.Entity.Function getAttrSum = new CorePackage.Entity.Function();
            type.Declare(getAttrSum, "getAttrSum", CorePackage.Global.AccessMode.EXTERNAL);
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

            uint getT = getAttrSum.addInstruction(new CorePackage.Execution.Getter(getAttrSum.GetParameter("this")));

            uint getAttrs = getAttrSum.addInstruction(new CorePackage.Execution.GetAttributes(type));
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
            getAttrSum.Call(new Dictionary<string, dynamic> { { "this", tvar.Value } });

            Console.WriteLine(getAttrSum.GetReturnValue("res"));
            Assert.IsTrue(getAttrSum.GetReturnValue("res") == 16);

            CorePackage.Entity.Function addOp = (CorePackage.Entity.Function)type.Declare(new CorePackage.Entity.Function(), "Add", CorePackage.Global.AccessMode.EXTERNAL);

            // Object Add(Object this, Objet RightOperand);
            type.SetFunctionAsMember("Add", CorePackage.Global.AccessMode.EXTERNAL);
            addOp.Declare(new CorePackage.Entity.Variable(type), CorePackage.Global.Operator.Right, CorePackage.Global.AccessMode.EXTERNAL);
            addOp.SetVariableAs(CorePackage.Global.Operator.Right, CorePackage.Entity.Function.VariableRole.PARAMETER);
            addOp.Declare(new CorePackage.Entity.Variable(type), CorePackage.Global.Operator.Result, CorePackage.Global.AccessMode.EXTERNAL);
            addOp.SetVariableAs(CorePackage.Global.Operator.Result, CorePackage.Entity.Function.VariableRole.RETURN);

            type.OverloadOperator(CorePackage.Global.Operator.Name.ADD, "Add");

            /*
             * 
             * result.x = this.x + RightOperand.x;
             * result.y = this.y + RightOperand.y;
             * result.z = this.z + RightOperand.z;
             * 
             */
          
            uint getThis = addOp.addInstruction(new CorePackage.Execution.Getter(addOp.GetParameter("this")));
            uint splitThis = addOp.addInstruction(new CorePackage.Execution.GetAttributes(type));
            addOp.LinkInstructionData(getThis, "reference", splitThis, "this");

            uint getROP = addOp.addInstruction(new CorePackage.Execution.Getter(addOp.GetParameter(CorePackage.Global.Operator.Right)));
            uint splitROP = addOp.addInstruction(new CorePackage.Execution.GetAttributes(type));
            addOp.LinkInstructionData(getROP, "reference", splitROP, "this");

            //this.x + RightOperand.x
            uint addX = addOp.addInstruction(new CorePackage.Execution.Operators.Add(integer, integer, integer));
            addOp.LinkInstructionData(splitThis, "x", addX, CorePackage.Global.Operator.Left);
            addOp.LinkInstructionData(splitROP, "x", addX, CorePackage.Global.Operator.Right);

            //this.y + RightOperand.y
            uint addY = addOp.addInstruction(new CorePackage.Execution.Operators.Add(integer, integer, integer));
            addOp.LinkInstructionData(splitThis, "y", addY, CorePackage.Global.Operator.Left);
            addOp.LinkInstructionData(splitROP, "y", addY, CorePackage.Global.Operator.Right);

            //this.z + RightOperand.z
            uint addZ = addOp.addInstruction(new CorePackage.Execution.Operators.Add(integer, integer, integer));
            addOp.LinkInstructionData(splitThis, "z", addZ, CorePackage.Global.Operator.Left);
            addOp.LinkInstructionData(splitROP, "z", addZ, CorePackage.Global.Operator.Right);

            uint getRes = addOp.addInstruction(new CorePackage.Execution.Getter(addOp.GetReturn(CorePackage.Global.Operator.Result)));
            uint splitRes = addOp.addInstruction(new CorePackage.Execution.GetAttributes(type));
            addOp.LinkInstructionData(getRes, "reference", splitRes, "this");

            uint setResult = addOp.addInstruction(new CorePackage.Execution.SetAttribute(type));
            addOp.LinkInstructionData(getRes, "reference", setResult, "this");
            addOp.LinkInstructionData(addX, CorePackage.Global.Operator.Result, setResult, "x");
            addOp.LinkInstructionData(addY, CorePackage.Global.Operator.Result, setResult, "y");
            addOp.LinkInstructionData(addZ, CorePackage.Global.Operator.Result, setResult, "z");

            addOp.setEntryPoint(setResult);

            var toadd = type.Instantiate();

            toadd["x"] = 12;
            toadd["y"] = 20;
            toadd["z"] = -42;

            tvar.Value["x"] = 3;
            tvar.Value["y"] = 42;
            tvar.Value["z"] = -29;

            var addition = type.OperatorAdd(toadd, tvar.Value);

            Assert.IsTrue(addition["x"] == 15);
            Assert.IsTrue(addition["y"] == 62);
            Assert.IsTrue(addition["z"] == -71);
        }
    }
}
