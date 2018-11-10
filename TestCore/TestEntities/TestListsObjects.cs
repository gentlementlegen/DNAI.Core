using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

            Assert.IsTrue(type.OperatorEqual(union, new List<int> { 0, 0, 42, 50 }));
            Assert.IsTrue(type.OperatorEqual(sub, new List<int> { 50 }));
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
        public void TestNestedList()
        {
            CorePackage.Entity.Type.ListType intlist = new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Integer);
            CorePackage.Entity.Type.ListType intlistlist = new CorePackage.Entity.Type.ListType(intlist);
            CorePackage.Execution.Append instruction = new CorePackage.Execution.Append(intlist);
            List<List<int>> val = new List<List<int>>();
            
            instruction.SetInputValue("array", val);
            instruction.SetInputValue("element", new List<int>());

            Assert.IsTrue(instruction.GetInputValue("array").Count == 0);
            Assert.IsTrue(instruction.GetInputValue("element").Count == 0);

            instruction.Execute();

            Assert.IsTrue(val.Count == 1);

            CorePackage.Execution.Append intlistappend = new CorePackage.Execution.Append(CorePackage.Entity.Type.Scalar.Integer);
            CorePackage.Execution.Operators.Access access = new CorePackage.Execution.Operators.Access(intlistlist, CorePackage.Entity.Type.Scalar.Integer, intlist);
            CorePackage.Execution.Getter getfrom = new CorePackage.Execution.Getter(new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 0));

            access.SetInputValue("LeftOperand", val);
            access.SetInputValue("RightOperand", 0);

            intlistappend.GetInput("array").LinkTo(access, "result");
            intlistappend.GetInput("element").LinkTo(getfrom, "reference");
            //intlistappend.SetInputValue("array", val[0]);

            Assert.IsTrue(intlistappend.GetInputValue("array").Count == 0);
            Assert.IsTrue(val[0].Count == 0);
            Assert.IsTrue(val[0] == intlistappend.GetInputValue("array"));

            intlistappend.Execute();

            Assert.IsTrue(intlistappend.GetInputValue("array").Count == 1);
            Assert.IsTrue(val[0].Count == 1);
            Assert.IsTrue(val[0] == intlistappend.GetInputValue("array"));

            List<dynamic> jsonlist = intlist.CreateFromJSON("[3, 9, 1, 2]");

            Assert.IsTrue(intlist.IsValueOfType(jsonlist));
            Assert.IsTrue(jsonlist[0] == 3);
            Assert.IsTrue(jsonlist[1] == 9);
            Assert.IsTrue(jsonlist[2] == 1);
            Assert.IsTrue(jsonlist[3] == 2);
        }

        [TestMethod]
        public void TestListRemove()
        {
            CorePackage.Entity.Type.ListType intlist = new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Integer);
            CorePackage.Execution.RemoveIndex ins = new CorePackage.Execution.RemoveIndex(CorePackage.Entity.Type.Scalar.Integer);
            List<int> data = new List<int> { 2, 0, 1 };

            ins.SetInputValue("array", data);
            ins.SetInputValue("index", 0);

            ins.Execute();

            Assert.IsTrue(data.Count == 2);
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
            type.SetFunctionAsMember("getAttrSum");

            CorePackage.Entity.Variable res = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer);
            res.Name = "res";
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
            type.SetFunctionAsMember("Add");
            CorePackage.Entity.Variable rgt = new CorePackage.Entity.Variable(type);
            rgt.Name = CorePackage.Global.Operator.Right;

            addOp.Declare(rgt, CorePackage.Global.Operator.Right, CorePackage.Global.AccessMode.EXTERNAL);
            addOp.SetVariableAs(CorePackage.Global.Operator.Right, CorePackage.Entity.Function.VariableRole.PARAMETER);

            CorePackage.Entity.Variable rs = new CorePackage.Entity.Variable(type);
            rs.Name = CorePackage.Global.Operator.Result;

            addOp.Declare(rs, CorePackage.Global.Operator.Result, CorePackage.Global.AccessMode.EXTERNAL);
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
        
        public class Pos
        {
            public class ZType
            {
                public double toto;
            }

            public float X;
            public List<double> Y;
            public ZType Z;
        }

        [TestMethod]
        public void TestObjectValues()
        {
            CorePackage.Entity.Type.ObjectType obj = new CorePackage.Entity.Type.ObjectType();
            CorePackage.Entity.Type.ObjectType ztype = new CorePackage.Entity.Type.ObjectType();

            ztype.AddAttribute("toto", CorePackage.Entity.Type.Scalar.Floating, CorePackage.Global.AccessMode.EXTERNAL);

            var ltype = new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Floating);

            obj.AddAttribute("X", CorePackage.Entity.Type.Scalar.Floating, CorePackage.Global.AccessMode.EXTERNAL);
            obj.AddAttribute("Y", ltype, CorePackage.Global.AccessMode.EXTERNAL);
            obj.AddAttribute("Z", ztype, CorePackage.Global.AccessMode.EXTERNAL);

            Dictionary<string, dynamic> dicValue = new Dictionary<string, dynamic>
            {
                { "X", 3.14f },
                { "Y", new List<double> { 4.12 } },
                { "Z", new Dictionary<string, dynamic> { { "toto", -3093.334 } } }
            };
            object jsonValue = JsonConvert.DeserializeObject(@"{
                ""X"": 3.14,
                ""Y"": [4.12],
                ""Z"": { ""toto"": -3093.334 }
            }");
            Pos realValue = new Pos
            {
                X = 3.14f,
                Y = new List<double> { 4.12 },
                Z = new Pos.ZType { toto = -3093.334 }
            };

            Assert.IsTrue(obj.IsValueOfType(dicValue));
            Assert.IsTrue(obj.IsValueOfType(jsonValue));
            Assert.IsTrue(obj.IsValueOfType(realValue));

            obj.SetAttributeValue(realValue, "X", 3.40);

            var jsonval = obj.CreateFromJSON(@"{
                ""X"": 3.14,
                ""Y"": [4.12],
                ""Z"": { ""toto"": -3093.334 }
            }");

            Assert.IsTrue(obj.GetAttributeValue(jsonval, "X") == 3.14);
            Assert.IsTrue(ltype.IsValueOfType(obj.GetAttributeValue(jsonval, "Y")));
            Assert.IsTrue(ztype.IsValueOfType(obj.GetAttributeValue(jsonval, "Z")));
        }
    }
}
