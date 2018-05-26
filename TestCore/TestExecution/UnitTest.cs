
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using CorePackage.Global;
using CorePackage.Entity;
using CorePackage.Entity.Type;

namespace CoreTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void moreOrLess()
        {
            Context moreOrLessCtx = new Context();

            EnumType cmp = new EnumType(Scalar.Integer);
            cmp.SetValue("MORE", new Variable(Scalar.Integer, 0));
            cmp.SetValue("LESS", new Variable(Scalar.Integer, 1));
            cmp.SetValue("NONE", new Variable(Scalar.Integer, 2));

            Variable ctx_min = new Variable(Scalar.Integer, 0);
            Variable ctx_max = new Variable(Scalar.Integer, 100);
            Variable ctx_lastGiven = new Variable(Scalar.Integer, -1);

            moreOrLessCtx.Declare(cmp, "COMPARISON", AccessMode.EXTERNAL);
            moreOrLessCtx.Declare(ctx_min, "min", AccessMode.INTERNAL);
            moreOrLessCtx.Declare(ctx_max, "max", AccessMode.INTERNAL);
            moreOrLessCtx.Declare(ctx_lastGiven, "lastGiven", AccessMode.INTERNAL);

            Function play = new Function();

            moreOrLessCtx.Declare(play, "Play", AccessMode.EXTERNAL);

            play.Declare(new Variable(cmp, cmp.GetValue("NONE").Value), "lastResult", AccessMode.EXTERNAL);
            play.SetVariableAs("lastResult", Function.VariableRole.PARAMETER);

            play.Declare(new Variable(Scalar.Integer, -1), "number", AccessMode.EXTERNAL);
            play.SetVariableAs("number", Function.VariableRole.RETURN);

            //if (lastResult == MORE)
            CorePackage.Execution.Operators.Equal lastResEqqMore = new CorePackage.Execution.Operators.Equal(cmp, cmp);
            lastResEqqMore.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(cmp.GetValue("MORE")), "reference");
            lastResEqqMore.GetInput("RightOperand").LinkTo(new CorePackage.Execution.Getter(play.GetParameter("lastResult")), "reference");

            CorePackage.Execution.If play_entrypoint = new CorePackage.Execution.If();

            play.setEntryPoint(play.addInstruction(play_entrypoint));
            play_entrypoint.GetInput("condition").LinkTo(lastResEqqMore, "result");

                //min = lastGiven
                CorePackage.Execution.Setter minEqLastGiven = new CorePackage.Execution.Setter(ctx_min);
                minEqLastGiven.GetInput("value").LinkTo(new CorePackage.Execution.Getter(ctx_lastGiven), "reference");
                play_entrypoint.LinkTo(0, minEqLastGiven); //"set" if true

                //if (lastResult == LESS)
                CorePackage.Execution.Operators.Equal lastResEqqLess = new CorePackage.Execution.Operators.Equal(cmp, cmp);
                lastResEqqLess.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(play.GetParameter("lastResult")), "reference");
                lastResEqqLess.GetInput("RightOperand").LinkTo(new CorePackage.Execution.Getter(cmp.GetValue("LESS")), "reference");
                CorePackage.Execution.If condition = new CorePackage.Execution.If();
                condition.GetInput("condition").LinkTo(lastResEqqLess, "result");
                play_entrypoint.LinkTo(1, condition); //"if" if false
            
                    //max = lastGiven
                    CorePackage.Execution.Setter maxEqLastGiven = new CorePackage.Execution.Setter(ctx_max);
                    maxEqLastGiven.GetInput("value").LinkTo(new CorePackage.Execution.Getter(ctx_lastGiven), "reference");
                    condition.LinkTo(0, maxEqLastGiven);

            //number = min/2 + max / 2
            CorePackage.Execution.Setter numCalculation = new CorePackage.Execution.Setter(play.GetReturn("number"));
            minEqLastGiven.LinkTo(0, numCalculation);
            condition.LinkTo(1, numCalculation);

            CorePackage.Execution.Operators.Add bigOp = new CorePackage.Execution.Operators.Add(
                Scalar.Integer,
                Scalar.Integer,
                Scalar.Integer);

            //min / 2
                CorePackage.Execution.Operators.Divide minmid = new CorePackage.Execution.Operators.Divide(
                    Scalar.Integer,
                    Scalar.Integer,
                    Scalar.Integer);
                minmid.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(ctx_min), "reference");
                minmid.SetInputValue("RightOperand", 2);
                bigOp.GetInput("LeftOperand").LinkTo(minmid, "result");

            //max / 2
                CorePackage.Execution.Operators.Divide maxmid = new CorePackage.Execution.Operators.Divide(
                    Scalar.Integer,
                    Scalar.Integer,
                    Scalar.Integer);
                maxmid.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(ctx_max), "reference");
                maxmid.SetInputValue("RightOperand", 2);
                bigOp.GetInput("RightOperand").LinkTo(maxmid, "result");

            numCalculation.GetInput("value").LinkTo(bigOp, "result");

            //if (number == lastGiven)
            condition = new CorePackage.Execution.If();
            CorePackage.Execution.Operators.Equal numberEqqLastGiven = new CorePackage.Execution.Operators.Equal(Scalar.Integer, Scalar.Integer);
            numberEqqLastGiven.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(play.GetReturn("number")), "reference");
            numberEqqLastGiven.GetInput("RightOperand").LinkTo(new CorePackage.Execution.Getter(ctx_lastGiven), "reference");
            condition.GetInput("condition").LinkTo(numberEqqLastGiven, "result");

            maxEqLastGiven.LinkTo(0, numCalculation);
            numCalculation.LinkTo(0, condition);
            
            //lastGiven = number
            CorePackage.Execution.Setter lastGivenEqNumber = new CorePackage.Execution.Setter(ctx_lastGiven);
            lastGivenEqNumber.GetInput("value").LinkTo(new CorePackage.Execution.Getter(play.GetReturn("number")), "reference");
            condition.LinkTo(1, lastGivenEqNumber);

            //if (lastResult == MORE)
            CorePackage.Execution.If last = new CorePackage.Execution.If();
            last.GetInput("condition").LinkTo(lastResEqqMore, "result");

            condition.LinkTo(0, last);

                //number += 1
                CorePackage.Execution.Setter numberPlusOne = new CorePackage.Execution.Setter(play.GetReturn("number"));

                CorePackage.Execution.Operators.Add numAddOne = new CorePackage.Execution.Operators.Add(Scalar.Integer, Scalar.Integer, Scalar.Integer);
                numAddOne.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(play.GetReturn("number")), "reference");
                numAddOne.SetInputValue("RightOperand", 1);
                numberPlusOne.GetInput("value").LinkTo(numAddOne, "result");
                last.LinkTo(0, numberPlusOne);

                //number -= 1
                CorePackage.Execution.Setter numberMinusOne = new CorePackage.Execution.Setter(play.GetReturn("number"));

                CorePackage.Execution.Operators.Substract numMinOne = new CorePackage.Execution.Operators.Substract(Scalar.Integer, Scalar.Integer, Scalar.Integer);
                numMinOne.GetInput("LeftOperand").LinkTo(new CorePackage.Execution.Getter(play.GetReturn("number")), "reference");
                numMinOne.SetInputValue("RightOperand", 1);
                numberMinusOne.GetInput("value").LinkTo(numMinOne, "result");
                last.LinkTo(1, numberMinusOne);

            //lastGiven = number (in any case)
            numberPlusOne.LinkTo(0, lastGivenEqNumber);
            numberMinusOne.LinkTo(0, lastGivenEqNumber);

            play.SetParameterValue("lastResult", cmp.GetValue("NONE").Value);

            System.Diagnostics.Debug.Write(play.ToDotFile());

            int mystery_number = 47;

            int i = 0;

            do
            {
                play.Call();

                string toprint = "IA give: " + play.GetReturn("number").Value.ToString();

                Debug.WriteLine(toprint);

                if (play.GetReturn("number").Value > mystery_number)
                {
                    play.SetParameterValue("lastResult", cmp.GetValue("LESS").Value);
                    Debug.WriteLine("==> It's less");
                }
                else if (play.GetReturn("number").Value < mystery_number)
                {
                    play.SetParameterValue("lastResult", cmp.GetValue("MORE").Value);
                    Debug.WriteLine("==> It's more");
                }
                else
                    break;
                ++i;
            } while (play.GetReturn("number").Value != mystery_number && i < 10);

            if (i == 10)
                throw new Exception("Failed to reach mystery number in less that 10 times");
            else
                Debug.Write("AI found the mystery number: " + mystery_number.ToString());
        }
    }
}
