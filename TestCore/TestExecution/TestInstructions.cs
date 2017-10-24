using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CoreTest
{
    [TestClass]
    public class TestInstructions
    {

        /// <summary>
        /// Test all the operators on integer values
        /// </summary>
        [TestMethod]
        public void TestOperators()
        {
            CorePackage.Entity.DataType opType = CorePackage.Entity.Type.Scalar.Integer;

            

            //binary combination operators : add, divide, xor, leftShift, etc...
            TestAuxiliary.HandleOperations<int>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Add(opType, opType, opType),
                    new CorePackage.Execution.Operators.Divide(opType, opType, opType),
                    new CorePackage.Execution.Operators.Multiplicate(opType, opType, opType),
                    new CorePackage.Execution.Operators.Substract(opType, opType, opType),
                    new CorePackage.Execution.Operators.Modulo(opType, opType, opType),
                    new CorePackage.Execution.Operators.BinaryAnd(opType, opType, opType),
                    new CorePackage.Execution.Operators.BinaryOr(opType, opType, opType),
                    new CorePackage.Execution.Operators.LeftShift(opType, opType, opType),
                    new CorePackage.Execution.Operators.RightShift(opType, opType, opType),
                    new CorePackage.Execution.Operators.Xor(opType, opType, opType)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    (CorePackage.Execution.Instruction toinit) => {
                        toinit.SetInputValue("LeftOperand", 4);
                        toinit.SetInputValue("RightOperand", 2);
                        return true;
                    }
                },
                new List<List<int>>
                {
                    new List<int>
                    {
                        6,
                        2,
                        8,
                        2,
                        0,
                        0,
                        6,
                        16,
                        1,
                        6
                    }
                });

            //binary logical operators : greater, less, equal, etc...
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Equal(opType, opType),
                    new CorePackage.Execution.Operators.Greater(opType, opType),
                    new CorePackage.Execution.Operators.GreaterEqual(opType, opType),
                    new CorePackage.Execution.Operators.Less(opType, opType),
                    new CorePackage.Execution.Operators.LessEqual(opType, opType),
                    new CorePackage.Execution.Operators.Different(opType, opType)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    delegate (CorePackage.Execution.Instruction toinit)
                    {
                        toinit.SetInputValue("LeftOperand", 4);
                        toinit.SetInputValue("RightOperand", 2);
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        false,
                        true,
                        true,
                        false,
                        false,
                        true
                    }
                });

            //unary operators : binary not, increment, etc...
            TestAuxiliary.HandleOperations<int>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.BinaryNot(opType, opType),
                    new CorePackage.Execution.Operators.Decrement(opType, opType), //not sure works
                    new CorePackage.Execution.Operators.Increment(opType, opType), //not sure works
                    new CorePackage.Execution.Operators.Inverse(opType, opType)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    delegate (CorePackage.Execution.Instruction toinit)
                    {
                        toinit.SetInputValue("Operand", 4);
                        return true;
                    }
                },
                new List<List<int>>
                {
                    new List<int>
                    {
                        -5,
                        3,
                        5,
                        -4
                    }
                });

            //logical operators: and, or, not
            TestAuxiliary.HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.And(),
                    new CorePackage.Execution.Operators.Or(),
                    new CorePackage.Execution.Operators.Not(CorePackage.Entity.Type.Scalar.Boolean)
                },
                new List<Func<CorePackage.Execution.Instruction, bool>>
                {
                    delegate (CorePackage.Execution.Instruction toinit)
                    {
                        if (toinit.GetType() == typeof(CorePackage.Execution.Operators.Not))
                        {
                            toinit.SetInputValue("Operand", true);
                        }
                        else
                        {
                            toinit.SetInputValue("LeftOperand", true);
                            toinit.SetInputValue("RightOperand", false);
                        }
                        return true;
                    }
                },
                new List<List<bool>>
                {
                    new List<bool>
                    {
                        false,
                        true,
                        false
                    }
                });

            //add a test for the access operator => leftOperand[rightOperand]
        }

        /// <summary>
        /// Execution method that test if, getter, setter, debug and function call instructions on integer values
        /// </summary>
        /// 
        /// <remarks>
        /// Corresponds to the following code :
        /// 
        /// <code>
        /// int i;
        /// int witness = 42;
        /// 
        /// function say_hello()
        /// {
        ///     print("Hello World !");
        ///     witness = 84;
        /// }
        /// 
        /// function say_bye()
        /// {
        ///     print("Goodbye World !");
        ///     witness = 0;
        /// }
        /// 
        /// if (i == 5)
        /// {
        ///     say_hello();
        /// }
        /// else
        /// {
        ///     say_bye();
        /// }
        /// </code>
        /// 
        /// We gonna test this function for :
        ///     - i = 4 => expected value of witness is 0
        ///     - i = 5 => expected value of witness is 84
        /// </remarks>
        [TestMethod]
        public void Test_if_getter_setter_debug_functionCall_Instructions()
        {
            //Function that will be executed
            CorePackage.Entity.Function test = new CorePackage.Entity.Function();

            //Variable used to check function validity
            CorePackage.Entity.Variable witness = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 42);

            //if (4 == 5)
            CorePackage.Execution.If f_cond = new CorePackage.Execution.If();
            CorePackage.Execution.Operators.Equal condition = new CorePackage.Execution.Operators.Equal(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer);
            condition.SetInputValue("LeftOperand", 4);
            condition.SetInputValue("RightOperand", 5);
            f_cond.GetInput("condition").LinkTo(condition, "result");

            CorePackage.Entity.Function say_hello = new CorePackage.Entity.Function();
            
            //print("Hello World !")
            CorePackage.Execution.Debug print_hello = new CorePackage.Execution.Debug(new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.String, "Hello World !"));
            //witness = 84
            CorePackage.Execution.Setter true_change = new CorePackage.Execution.Setter(witness);
            true_change.SetInputValue("value", 84);
            print_hello.LinkTo(0, true_change);
            say_hello.entrypoint = print_hello;

            //If the condition is true, then do print_hello
            f_cond.Then(new CorePackage.Execution.FunctionCall(say_hello));

            CorePackage.Entity.Function say_bye = new CorePackage.Entity.Function();
            //print("Goodbye World !")
            CorePackage.Execution.Debug print_goodbye = new CorePackage.Execution.Debug(new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.String, "Goodbye World !"));
            //witness = 0
            CorePackage.Execution.Setter false_change = new CorePackage.Execution.Setter(witness);
            false_change.SetInputValue("value", 0);
            print_goodbye.LinkTo(0, false_change);
            say_bye.entrypoint = print_goodbye;

            //Else, do print_goodbye
            f_cond.Else(new CorePackage.Execution.FunctionCall(say_bye));

            //Set the function entry point before calling it
            test.entrypoint = f_cond;

            //In this call, it will check that 4 is equal to 5, then it will execute print_goodbye and false_change
            test.Call();

            //So the witness value is expected to be 0
            if (witness.Value != 0)
                throw new Exception("Failed: Witness have to be equal to 0");

            //To change the condition result, we will set the right operand of the operation to 4
            condition.SetInputValue("RightOperand", 4);

            //Then, function call will check if 4 is equal to 4 in order to execute print_hello
            test.Call();

            //So the witness value is exepected to be 84
            if (witness.Value != 84)
                throw new Exception("Failed: Witness have to be equal to 84");

            System.Diagnostics.Debug.Write(test.ToDotFile());
        }

        /// <summary>
        /// This method test while instruction this duly graph corresponds to the following code
        /// 
        /// void whileTester()
        /// {
        ///     int i = 0;
        /// 
        ///     while (i != 10)
        ///         i = i + 1;
        ///     i = 42;
        /// }
        /// 
        /// In the end, test function will check if i is equal to 42
        /// </summary>
        [TestMethod]
        public void Test_while()
        {
            CorePackage.Entity.Function whileTester = new CorePackage.Entity.Function();
            CorePackage.Entity.Variable i = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 0);

            //while(i != 10)
            CorePackage.Execution.While loop = new CorePackage.Execution.While();

            CorePackage.Execution.Operators.Different whcondition = new CorePackage.Execution.Operators.Different(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer);

            whcondition.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(i), "reference");
            whcondition.SetInputValue("RightOperand", 10);

            loop.GetInput("condition").LinkTo(whcondition, "result");

            //  i = i + 1;
            CorePackage.Execution.Setter ipp = new CorePackage.Execution.Setter(i);

            CorePackage.Execution.Operators.Add ipone = new CorePackage.Execution.Operators.Add(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer);

            ipone.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(i), "reference");
            ipone.SetInputValue("RightOperand", 1);

            ipp.GetInput("value").LinkTo(ipone, "result");

            loop.Do(ipp);

            //i = 42;
            CorePackage.Execution.Setter finalset = new CorePackage.Execution.Setter(i);

            finalset.SetInputValue("value", 42);

            loop.Done(finalset);

            //===============================
        
            whileTester.entrypoint = loop;
            whileTester.Call();

            Assert.IsTrue(i.Value == 42);
        }

        /// <summary>
        /// This method test foreach instruction this duly graph corresponds to the following code
        /// 
        /// void TestForeach()
        /// {
        ///     List<int> l = new List<int> {1, 2, 3};
        /// 
        ///     foreach (var v in l)
        ///         v = 42;
        /// }
        /// 
        /// </summary>
        [TestMethod]
        public void TestForeach()
        {
            CorePackage.Entity.Function whileTester = new CorePackage.Entity.Function();
            CorePackage.Entity.Variable l = new CorePackage.Entity.Variable(new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Floating), new List<double> { 1.0, 2.0, 42.0 });

            //while(i != 10)
            CorePackage.Execution.Foreach loop = new CorePackage.Execution.Foreach();
            loop.ContainerType = CorePackage.Entity.Type.Scalar.Floating;

            //CorePackage.Execution.Operators.Different whcondition = new CorePackage.Execution.Operators.Different(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer);

            //whcondition.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(i), "reference");
            //whcondition.SetInputValue("RightOperand", 10);

            //loop.GetInput("condition").LinkTo(whcondition, "result");
            loop.GetInput("array").LinkTo(new CorePackage.Execution.Getter(l), "reference");

            ////  i = i + 1;
            CorePackage.Entity.Variable i = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer);
            CorePackage.Entity.Variable j = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Floating);
            CorePackage.Execution.Setter ipp = new CorePackage.Execution.Setter(i);
            CorePackage.Execution.Setter ipp2 = new CorePackage.Execution.Setter(j);

            //CorePackage.Execution.Operators.Add ipone = new CorePackage.Execution.Operators.Add(CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer, CorePackage.Entity.Type.Scalar.Integer);

            //ipone.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(i), "reference");
            //ipone.SetInputValue("RightOperand", 1);

            //ipp.GetInput("value").LinkTo(ipone, "result");
            ipp.GetInput("value").LinkTo(loop, "index");
            ipp2.GetInput("value").LinkTo(loop, "element");

            //loop.Do(ipp);
            loop.Do(ipp);

            ////i = 42;
            //CorePackage.Execution.Setter finalset = new CorePackage.Execution.Setter(i);

            //finalset.SetInputValue("value", 42);

            //loop.Done(finalset);

            ////===============================

            whileTester.entrypoint = loop;
            whileTester.Call();
            whileTester.Call();

            Assert.IsTrue(i.Value == 3);
            //Assert.IsTrue(j.Value == 42);
        }

        [TestMethod]
        public void TestAdd()
        {
            CorePackage.Entity.Function whileTester = new CorePackage.Entity.Function();

            CorePackage.Entity.Variable j = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Floating);
            CorePackage.Entity.Variable l = new CorePackage.Entity.Variable(new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Floating), new List<double> { 1.0, 2.0, 42.0 });

            CorePackage.Execution.Add add = new CorePackage.Execution.Add();
            add.ContainerType = CorePackage.Entity.Type.Scalar.Floating;

            add.GetInput("array").LinkTo(new CorePackage.Execution.Getter(l), "reference");
            add.GetInput("element").LinkTo(new CorePackage.Execution.Getter(j), "reference");
            var n = add.GetOutput("count").Value;

            whileTester.entrypoint = add;
            whileTester.Call();
            Assert.IsTrue(n.definition.Value == 4);
        }

        [TestMethod]
        public void TestInsert()
        {
            CorePackage.Entity.Function whileTester = new CorePackage.Entity.Function();

            CorePackage.Entity.Variable j = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Floating);
            CorePackage.Entity.Variable idx = new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.Integer, 2);
            CorePackage.Entity.Variable l = new CorePackage.Entity.Variable(new CorePackage.Entity.Type.ListType(CorePackage.Entity.Type.Scalar.Floating), new List<double> { 1.0, 2.0, 42.0 });

            CorePackage.Execution.Insert insert = new CorePackage.Execution.Insert
            {
                ContainerType = CorePackage.Entity.Type.Scalar.Floating
            };

            insert.GetInput("array").LinkTo(new CorePackage.Execution.Getter(l), "reference");
            insert.GetInput("element").LinkTo(new CorePackage.Execution.Getter(j), "reference");
            insert.GetInput("index").LinkTo(new CorePackage.Execution.Getter(idx), "reference");

            var n = insert.GetOutput("count").Value;

            whileTester.entrypoint = insert;
            whileTester.Call();
            Assert.IsTrue(n.definition.Value == 4);
        }
    }
}
