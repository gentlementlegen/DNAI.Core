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
                        i.SetInputValue(CorePackage.Global.Operator.Left, true);
                        i.SetInputValue(CorePackage.Global.Operator.Right, true);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, true);
                        i.SetInputValue(CorePackage.Global.Operator.Right, false);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, false);
                        i.SetInputValue(CorePackage.Global.Operator.Right, true);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, false);
                        i.SetInputValue(CorePackage.Global.Operator.Right, false);
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
                        i.SetInputValue(CorePackage.Global.Operator.Left, 42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, -42);
                        return true;
                    },
                    //equal
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 42);
                        return true;
                    },
                    //less
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, -42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 42);
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
                        i.SetInputValue(CorePackage.Global.Operator.Left, 42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, -42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 0);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, -42);
                        i.SetInputValue(CorePackage.Global.Operator.Right, -42);
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
            TestAuxiliary.HandleOperations<int>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.BinaryNot(integer, integer),
                    new CorePackage.Execution.Operators.Decrement(integer, integer),
                    new CorePackage.Execution.Operators.Increment(integer, integer),
                    new CorePackage.Execution.Operators.Inverse(integer, integer)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", 42);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", -42);
                        return true;
                    }
                },
                new List<List<int>>
                {
                    new List<int>
                    {
                        -43,
                        41,
                        43,
                        -42
                    },
                    new List<int>
                    {
                        41,
                        -43,
                        -41,
                        42
                    }
                });
        }

        [TestMethod]
        public void TestFloating()
        {
            CorePackage.Entity.DataType floating = CorePackage.Entity.Type.Scalar.Floating;

            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
            {
                new CorePackage.Execution.Operators.Different(floating, floating),
                new CorePackage.Execution.Operators.Equal(floating, floating),
                new CorePackage.Execution.Operators.Greater(floating, floating),
                new CorePackage.Execution.Operators.GreaterEqual(floating, floating),
                new CorePackage.Execution.Operators.Less(floating, floating),
                new CorePackage.Execution.Operators.LessEqual(floating, floating)
            },
                new List<Func<CorePackage.Execution.Instruction, bool>>
            {
                (CorePackage.Execution.Instruction i) =>
                {
                    i.SetInputValue(CorePackage.Global.Operator.Left, 3.14);
                    i.SetInputValue(CorePackage.Global.Operator.Right, 4.2);
                    return true;
                },
                (CorePackage.Execution.Instruction i) =>
                {
                    i.SetInputValue(CorePackage.Global.Operator.Left, 4.2);
                    i.SetInputValue(CorePackage.Global.Operator.Right, 4.2);
                    return true;
                },
                (CorePackage.Execution.Instruction i) =>
                {
                    i.SetInputValue(CorePackage.Global.Operator.Left, 4.2);
                    i.SetInputValue(CorePackage.Global.Operator.Right, 3.14);
                    return true;
                }
            },
                new List<List<bool>>
            {
                new List<bool>
                {
                    true,
                    false,
                    false,
                    false,
                    true,
                    true
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
                    true,
                    true,
                    false,
                    false
                }
            });

            //Add, Divide, Multiplicate, Substract, Modulo
            TestAuxiliary.HandleOperations<double>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Add(floating, floating, floating),
                    new CorePackage.Execution.Operators.Divide(floating, floating, floating),
                    new CorePackage.Execution.Operators.Multiplicate(floating, floating, floating),
                    new CorePackage.Execution.Operators.Substract(floating, floating, floating),
                    new CorePackage.Execution.Operators.Modulo(floating, floating, floating)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 3.14);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 4.2);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 0.0);
                        i.SetInputValue(CorePackage.Global.Operator.Right, 4.2);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 3.14);
                        i.SetInputValue(CorePackage.Global.Operator.Right, -4.2);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, -3.14);
                        i.SetInputValue(CorePackage.Global.Operator.Right, -4.2);
                        return true;
                    }
                },
                new List<List<double>>
                {
                    new List<double>
                    {
                        3.14 + 4.2,
                        3.14 / 4.2,
                        3.14 * 4.2,
                        3.14 - 4.2,
                        3.14 % 4.2
                    },
                    new List<double>
                    {
                        0 + 4.2,
                        0 / 4.2,
                        0 * 4.2,
                        0 - 4.2,
                        0 % 4.2
                    },
                    new List<double>
                    {
                        3.14 + -4.2,
                        3.14 / -4.2,
                        3.14 * -4.2,
                        3.14 - -4.2,
                        3.14 % -4.2
                    },
                    new List<double>
                    {
                        -3.14 + -4.2,
                        -3.14 / -4.2,
                        -3.14 * -4.2,
                        -3.14 - -4.2,
                        -3.14 % -4.2
                    }
                });

            //Decrement, Increment, Inverse
            TestAuxiliary.HandleOperations<double>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Decrement(floating, floating),
                    new CorePackage.Execution.Operators.Increment(floating, floating),
                    new CorePackage.Execution.Operators.Inverse(floating, floating)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", 4.2);
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue("Operand", -4.2);
                        return true;
                    }
                },
                new List<List<double>>
                {
                    new List<double>
                    {
                        3.2,
                        5.2,
                        -4.2
                    },
                    new List<double>
                    {
                        -5.2,
                        -3.2,
                        4.2
                    }
                });
        }

        [TestMethod]
        public void TestCharacter()
        {
            CorePackage.Entity.DataType character = CorePackage.Entity.Type.Scalar.Character;

            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Different(character, character),
                    new CorePackage.Execution.Operators.Equal(character, character),
                    new CorePackage.Execution.Operators.Greater(character, character),
                    new CorePackage.Execution.Operators.GreaterEqual(character, character),
                    new CorePackage.Execution.Operators.Less(character, character),
                    new CorePackage.Execution.Operators.LessEqual(character, character)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 'A');
                        i.SetInputValue(CorePackage.Global.Operator.Right, 'R');
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 'A');
                        i.SetInputValue(CorePackage.Global.Operator.Right, 'A');
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, 'R');
                        i.SetInputValue(CorePackage.Global.Operator.Right, 'A');
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        true,
                        false,
                        false,
                        false,
                        true,
                        true
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
                        true,
                        true,
                        false,
                        false
                    }
                });
        }

        [TestMethod]
        public void TestString()
        {
            CorePackage.Entity.DataType my_string = CorePackage.Entity.Type.Scalar.String;
            
            //Different, Equal, Greater, GreaterEqual, Less, LessEqual
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Different(my_string, my_string),
                    new CorePackage.Execution.Operators.Equal(my_string, my_string)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, "duly");
                        i.SetInputValue(CorePackage.Global.Operator.Right, "apero");
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, "duly");
                        i.SetInputValue(CorePackage.Global.Operator.Right, "duly");
                        return true;
                    },
                    (CorePackage.Execution.Instruction i) =>
                    {
                        i.SetInputValue(CorePackage.Global.Operator.Left, "apero");
                        i.SetInputValue(CorePackage.Global.Operator.Right, "duly");
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        true,
                        false
                    },
                    new List<bool>
                    {
                        false,
                        true
                    },
                    new List<bool>
                    {
                        true,
                        false
                    }
                });

            //Add
        }
        
        [TestMethod]
        public void ScalarOperators()
        {
            CorePackage.Entity.DataType
                integer = CorePackage.Entity.Type.Scalar.Integer,
                floating = CorePackage.Entity.Type.Scalar.Floating,
                character = CorePackage.Entity.Type.Scalar.Character,
                boolean = CorePackage.Entity.Type.Scalar.Boolean,
                stringc = CorePackage.Entity.Type.Scalar.String;

            //Test integer

            Assert.IsTrue(integer.OperatorAdd(3, 4) == 7);
            Assert.IsTrue(integer.OperatorSub(3, 4) == -1);
            Assert.IsTrue(integer.OperatorMul(3, 4) == 12);
            Assert.IsTrue(integer.OperatorDiv(8, 4) == 2);
            Assert.IsTrue(integer.OperatorMod(7, 3) == 1);
            Assert.IsTrue(integer.OperatorGt(4, 3));
            Assert.IsTrue(integer.OperatorGtEq(4, 3) && integer.OperatorGtEq(4, 4));
            Assert.IsTrue(integer.OperatorLt(3, 4));
            Assert.IsTrue(integer.OperatorLtEq(3, 4) && integer.OperatorLtEq(4, 4));
            Assert.IsTrue(integer.OperatorEqual(4, 4));
            Assert.IsTrue(integer.OperatorBAnd(0b1111, 0b1010) == 0b1010);
            Assert.IsTrue(integer.OperatorBOr(0b0000, 0b1010) == 0b1010);
            Assert.IsTrue(integer.OperatorRightShift(0b1000, 3) == 0b1);
            Assert.IsTrue(integer.OperatorLeftShift(0b1, 3) == 0b1000);
            Assert.IsTrue(integer.OperatorXor(0b1100, 0b1010) == 0b0110);
            Assert.IsTrue(integer.OperatorBNot(0xFFFFFFFF) == 0b0);

            //Test floating

            Assert.IsTrue(floating.OperatorAdd(3.14, 4.13) == 7.27);
            
            Assert.IsTrue(Math.Round(floating.OperatorSub(3.14, 0.28), 2) == 2.86);
            Assert.IsTrue(Math.Round(floating.OperatorMul(3.14, 4.13), 4) == 12.9682);
            Assert.IsTrue(Math.Round(floating.OperatorDiv(3.14, 3.2), 5) == 0.98125);
            Assert.IsTrue(floating.OperatorGt(4.28, 3.14));
            Assert.IsTrue(floating.OperatorGtEq(4.28, 3.14) && floating.OperatorGtEq(4.28, 4.28));
            Assert.IsTrue(floating.OperatorLt(3.14, 4.28));
            Assert.IsTrue(floating.OperatorLtEq(3.14, 4.28) && floating.OperatorLtEq(3.14, 3.14));
            Assert.IsTrue(floating.OperatorEqual(3.14, 3.14));

            //Test character

            Assert.IsTrue(character.OperatorGt('c', 'a'));
            Assert.IsTrue(character.OperatorGtEq('c', 'a') && character.OperatorGtEq('c', 'c'));
            Assert.IsTrue(character.OperatorLt('a', 'c'));
            Assert.IsTrue(character.OperatorLtEq('a', 'c') && character.OperatorLtEq('a', 'a'));
            Assert.IsTrue(character.OperatorEqual('a', 'a'));

            //Test boolean

            Assert.IsTrue(
                boolean.OperatorBAnd(true, true)
                && !boolean.OperatorBAnd(true, false)
                && !boolean.OperatorBAnd(false, true)
                && !boolean.OperatorBAnd(false, false));
            Assert.IsTrue(
                boolean.OperatorBOr(true, true)
                && boolean.OperatorBOr(true, false)
                && boolean.OperatorBOr(false, true)
                && !boolean.OperatorBOr(false, false));
            Assert.IsTrue(
                !boolean.OperatorXor(true, true)
                && boolean.OperatorXor(true, false)
                && boolean.OperatorXor(false, true)
                && !boolean.OperatorXor(false, false));

            //Test string

            Assert.IsTrue(stringc.OperatorAdd("salut", "salut") == "salutsalut");
            Assert.IsTrue(stringc.OperatorEqual("hello", "hello"));
            Assert.IsTrue(stringc.OperatorAccess("salut", 3) == 'u');
        }
    }
}
