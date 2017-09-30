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
        private void HandleOperations<T>(List<CorePackage.Execution.Operator> to_test, List<T> expected, Func<CorePackage.Execution.Instruction, bool> init)
        {
            if (to_test.Count != expected.Count)
                throw new Exception("Test count have to be equal to expected results count");
            for (int i = 0; i < expected.Count; i++)
            {
                init.DynamicInvoke(to_test[i]);
                if (to_test[i].GetOutput("result").Value.definition.Value != expected[i])
                    throw new Exception("Invalid result for index " + i.ToString() + ": Expected " + expected[i].ToString() + " got " + ((T)to_test[i].GetOutput("result").Value.definition.Value).ToString());
            }
        }

        /// <summary>
        /// Test all the operators on integer values
        /// </summary>
        [TestMethod]
        public void TestOperators()
        {
            CorePackage.Entity.DataType opType = CorePackage.Entity.Type.Scalar.Integer;

            //binary combination operators
            HandleOperations<int>(
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
                },
                delegate(CorePackage.Execution.Instruction toinit)
                {
                    toinit.SetInputValue("LeftOperand", 4);
                    toinit.SetInputValue("RightOperand", 2);
                    return true;
                });

            //binary logical operators
            HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.Equal(opType, opType),
                    new CorePackage.Execution.Operators.Greater(opType, opType),
                    new CorePackage.Execution.Operators.GreaterEqual(opType, opType),
                    new CorePackage.Execution.Operators.Less(opType, opType),
                    new CorePackage.Execution.Operators.LessEqual(opType, opType),
                    new CorePackage.Execution.Operators.Different(opType, opType)
                },
                new List<bool>
                {
                    false,
                    true,
                    true,
                    false,
                    false,
                    true
                },
                delegate (CorePackage.Execution.Instruction toinit)
                {
                    toinit.SetInputValue("LeftOperand", 4);
                    toinit.SetInputValue("RightOperand", 2);
                    return true;
                });

            //unary operators
            HandleOperations<int>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.BinaryNot(opType, opType),
                    new CorePackage.Execution.Operators.Decrement(opType, opType),
                    new CorePackage.Execution.Operators.Increment(opType, opType),
                    new CorePackage.Execution.Operators.Inverse(opType, opType)
                },
                new List<int>
                {
                    -5,
                    3,
                    5,
                    -4
                },
                delegate (CorePackage.Execution.Instruction toinit)
                {
                    toinit.SetInputValue("Operand", 4);
                    return true;
                });

            //logical operators
            HandleOperations<bool>(
                new List<CorePackage.Execution.Operator>
                {
                    new CorePackage.Execution.Operators.And(),
                    new CorePackage.Execution.Operators.Or(),
                    new CorePackage.Execution.Operators.Not(CorePackage.Entity.Type.Scalar.Boolean)
                },
                new List<bool>
                {
                    false,
                    true,
                    false
                },
                delegate(CorePackage.Execution.Instruction toinit)
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
                });
        }

        /// <summary>
        /// Test execution of a condition that set a variable
        /// </summary>
        [TestMethod]
        public void TestExecutionRefreshAsync()
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

            //print("Hello World !")
            CorePackage.Execution.Debug print_hello = new CorePackage.Execution.Debug(new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.String, "Hello World !"));
            //witness = 84
            CorePackage.Execution.Setter true_change = new CorePackage.Execution.Setter(witness);
            true_change.SetInputValue("value", 84);
            print_hello.LinkTo(0, true_change);

            //If the condition is true, then do print_hello
            f_cond.Then(print_hello);

            //print("Goodbye World !")
            CorePackage.Execution.Debug print_goodbye = new CorePackage.Execution.Debug(new CorePackage.Entity.Variable(CorePackage.Entity.Type.Scalar.String, "Goodbye World !"));
            //witness = 0
            CorePackage.Execution.Setter false_change = new CorePackage.Execution.Setter(witness);
            false_change.SetInputValue("value", 0);
            print_goodbye.LinkTo(0, false_change);

            //Else, do print_goodbye
            f_cond.Else(print_goodbye);

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
    }
}
