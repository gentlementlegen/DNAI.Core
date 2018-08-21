using System;
using System.Collections.Generic;
using Core.Plugin.Unity.Editor.Conditions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestUnityPlugin
{
    [TestClass]
    public class UnitTest2
    {
        //[TestMethod]
        public void TestConditions()
        {
            List<ACondition> cdts = new List<ACondition>();
            ConditionInput<int> i = new ConditionInput<int>
            {
                Value = 42
            };

            var intCdt = new ConditionEvaluator
            {
                Condition = ConditionEvaluator.CONDITION.EQUAL,
                Input = 42
            };
            intCdt.SetRefOutput(i);
            //cdts.Add(intCdt);

            Assert.IsTrue(ACondition.EvaluateSet(cdts), "1 Conditions are not satisfied");

            i.Value = 0;
            Assert.IsFalse(ACondition.EvaluateSet(cdts), "2 Conditions are not satisfied");
        }

        //[TestMethod]
        public void TestConditionCallback()
        {
            List<ACondition> cdts = new List<ACondition>();
            ConditionInput<int> i = 128;

            var intCdt = new ConditionEvaluator
            {
                Condition = ConditionEvaluator.CONDITION.MORE,
                Input = 127
            };
            intCdt.SetRefOutput(i);
            //intCdt.Callback = () => intCdt.Input++;

            //cdts.Add(intCdt);

            Assert.IsTrue(ACondition.EvaluateSet(cdts), "1 Conditions are not satisfied");
            //intCdt.Callback();
            Assert.IsFalse(ACondition.EvaluateSet(cdts), "2 Conditions are not satisfied");
        }
    }
}
