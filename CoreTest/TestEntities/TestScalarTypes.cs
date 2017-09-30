using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTest.TestEntities
{
    [TestClass]
    public class TestScalarTypes
    {
        [TestMethod]
        public void TestBoolean()
        {
            //And, Or, Equal, Different
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.And(),
                    new CorePackage.Execution.Operators.Or(),
                    new CorePackage.Execution.Operators.Equal(CorePackage.Entity.Type.Scalar.Boolean, CorePackage.Entity.Type.Scalar.Boolean),
                    new CorePackage.Execution.Operators.Different(CorePackage.Entity.Type.Scalar.Boolean, CorePackage.Entity.Type.Scalar.Boolean)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", true);
                        i.SetInputValue("RightOperand", true);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", true);
                        i.SetInputValue("RightOperand", false);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", false);
                        i.SetInputValue("RightOperand", true);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", false);
                        i.SetInputValue("RightOperand", false);
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        true,
                        true,
                        true,
                        false
                    },
                    new List<bool>
                    {
                        false,
                        true,
                        false,
                        true
                    },
                    new List<bool>
                    {
                        false,
                        true,
                        false,
                        true
                    },
                    new List<bool>
                    {
                        false,
                        false,
                        true,
                        false
                    }
                });

            //Not
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Not(CorePackage.Entity.Type.Scalar.Boolean)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", true);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", false);
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        false
                    },
                    new List<bool>
                    {
                        true
                    }
                });
        }

        [TestMethod]
        public void TestInteger()
        {
            CorePackage.Entity.DataType integer = CorePackage.Entity.Type.Scalar.Integer;

            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Different(integer, integer),
                    new CorePackage.Execution.Operators.Equal(integer, integer),
                    new CorePackage.Execution.Operators.Greater(integer, integer),
                    new CorePackage.Execution.Operators.GreaterEqual(integer, integer),
                    new CorePackage.Execution.Operators.Less(integer, integer),
                    new CorePackage.Execution.Operators.LessEqual(integer, integer)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    //greater
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", 42);
                        i.SetInputValue("RightOperand", -42);
                        return true;
                    },
                    //equal
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", 42);
                        i.SetInputValue("RightOperand", 42);
                        return true;
                    },
                    //less
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", -42);
                        i.SetInputValue("RightOperand", 42);
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        true,
                        false,
                        true,
                        true,
                        false,
                        false
                    },
                    new List<bool>
                    {
                        false,
                        true,
                        false,
                        true,
                        false,
                        true
                    },
                    new List<bool>
                    {
                        true,
                        false,
                        false,
                        false,
                        true,
                        true
                    }
                }
            );

            //Add, BinaryAnd, BinaryOr, Divide, LeftShift, Modulo, Multiplicate, RightShift, Substract, Xor
            TestAuxiliary.HandleOperations<int>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Add(integer, integer, integer),
                    new CorePackage.Execution.Operators.BinaryAnd(integer, integer, integer),
                    new CorePackage.Execution.Operators.BinaryOr(integer, integer, integer),
                    new CorePackage.Execution.Operators.Divide(integer, integer, integer),
                    new CorePackage.Execution.Operators.LeftShift(integer, integer, integer),
                    new CorePackage.Execution.Operators.Modulo(integer, integer, integer),
                    new CorePackage.Execution.Operators.Multiplicate(integer, integer, integer),
                    new CorePackage.Execution.Operators.RightShift(integer, integer, integer),
                    new CorePackage.Execution.Operators.Substract(integer, integer, integer),
                    new CorePackage.Execution.Operators.Xor(integer, integer, integer)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", 42);
                        i.SetInputValue("RightOperand", -42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", 0);
                        i.SetInputValue("RightOperand", 42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", 42);
                        i.SetInputValue("RightOperand", 42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("LeftOperand", -42);
                        i.SetInputValue("RightOperand", -42);
                        return true;
                    }
                },
                new List<List<int>>
                {
                    new List<int>
                    {
                        0,
                        2,
                        -2,
                        -1,
                        176160768,
                        0,
                        -1764,
                        0,
                        84,
                        -4
                    },
                    new List<int>
                    {
                        42,
                        0,
                        42,
                        0,
                        0,
                        0,
                        0,
                        0,
                        -42,
                        42
                    },
                    new List<int>
                    {
                        84,
                        42,
                        42,
                        1,
                        43008,
                        0,
                        1764,
                        0,
                        0,
                        0
                    },
                    new List<int>
                    {
                        -84,
                        -42,
                        -42,
                        1,
                        -176160768,
                        0,
                        1764,
                        -1,
                        0,
                        0
                    }
                }
            );

            //BinaryNot, Decrement, Increment, Inverse
        }

        [TestMethod]
        public void TestFloating()
        {
            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            //Add, Divide, Multiplicate, Substract, Modulo
            //Decrement, Increment, Inverse
        }

        [TestMethod]
        public void TestCharacter()
        {
            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
        }

        [TestMethod]
        public void TestString()
        {
            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            //Add
        }
    }
}
