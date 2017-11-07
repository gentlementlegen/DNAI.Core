using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Entity;
using CorePackage.Global;
using CorePackage.Entity.Type;
using static CoreControl.InstructionFactory;
using System.Collections.Generic;
using CorePackage.Execution;

namespace TestControler
{
    [TestClass]
    public class ControllerTester
    {
        [TestMethod]
        public void ControllerMoreOrLess()
        {
            CoreControl.Controler controller = new CoreControl.Controler();
            List<uint> empty = new List<uint>();
            uint integer = (uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE;

            //declaring moreOrLess context in global context
            uint ctx = controller.declare<Context, IContext>(0, "moreOrLess", AccessMode.EXTERNAL);

            //declaring global variables min, max and lastGiven in moreOrLess context
            uint min = controller.declare<Variable>(ctx, "min", AccessMode.INTERNAL);
            controller.setVariableType(min, integer);
            controller.setVariableValue(min, 0);
            uint max = controller.declare<Variable>(ctx, "max", AccessMode.INTERNAL);
            controller.setVariableType(max, integer);
            controller.setVariableValue(max, 100);
            uint lastGiven = controller.declare<Variable>(ctx, "lastGiven", AccessMode.INTERNAL);
            controller.setVariableType(lastGiven, integer);
            controller.setVariableValue(lastGiven, -1);

            //declaring enumeration COMPARISON in moreOrLess context
            uint COMPARISON = controller.declare<EnumType, DataType>(ctx, "COMPARISON", AccessMode.EXTERNAL);
            controller.setEnumerationValue(COMPARISON, "MORE", 0);
            controller.setEnumerationValue(COMPARISON, "LESS", 1);
            controller.setEnumerationValue(COMPARISON, "NONE", 2);

            //declaring function play in moreOrLess context
            uint play = controller.declare<Function>(ctx, "Play", AccessMode.EXTERNAL);

            //declaring parameter lastResult in play function
            uint play_lastResult = controller.declare<Variable>(play, "lastResult", AccessMode.EXTERNAL);
            controller.setFunctionParameter(play, "lastResult");
            controller.setVariableType(play_lastResult, COMPARISON);
            controller.setVariableValue(play_lastResult, controller.getEnumerationValue(COMPARISON, "NONE"));

            //declaring return result in play function
            uint play_result = controller.declare<Variable>(play, "result", AccessMode.EXTERNAL);
            controller.setFunctionReturn(play, "result");
            controller.setVariableType(play_result, integer);

            uint split_COMPARISON = controller.addInstruction(play, INSTRUCTION_ID.ENUM_SPLITTER, new List<uint> { COMPARISON });
            uint get_last_result = controller.addInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { play_lastResult });

            //if (lastResult == COMPARISION::MORE)
            uint lr_eq_more = controller.addInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.linkInstructionData(play, split_COMPARISON, "MORE", lr_eq_more, "LeftOperand");
            controller.linkInstructionData(play, get_last_result, "reference", lr_eq_more, "RightOperand");
            uint if_lr_eq_more = controller.addInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.linkInstructionData(play, lr_eq_more, "result", if_lr_eq_more, "condition");

            //min = lastGiven
            uint get_lastGiven = controller.addInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { lastGiven });
            uint set_min = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { min });
            controller.linkInstructionData(play, get_lastGiven, "reference", set_min, "value");
            
            //if (lastResult == COMPARISON::LESS)
            uint lr_eq_less = controller.addInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.linkInstructionData(play, get_last_result, "reference", lr_eq_less, "LeftOperand");
            controller.linkInstructionData(play, split_COMPARISON, "LESS", lr_eq_less, "RightOperand");
            uint if_lr_eq_less = controller.addInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.linkInstructionData(play, lr_eq_less, "result", if_lr_eq_less, "condition");
            
            //max = lastGiven
            uint set_max = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { max });
            controller.linkInstructionData(play, get_lastGiven, "reference", set_max, "value");

            //min / 2
            uint get_min = controller.addInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { min });
            uint min_half = controller.addInstruction(play, INSTRUCTION_ID.DIV, new List<uint> { integer, integer, integer });
            controller.linkInstructionData(play, get_min, "reference", min_half, "LeftOperand");
            controller.setInstructionInputValue(play, min_half, "RightOperand", 2);

            //max / 2
            uint get_max = controller.addInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { max });
            uint max_half = controller.addInstruction(play, INSTRUCTION_ID.DIV, new List<uint> { integer, integer, integer });
            controller.linkInstructionData(play, get_max, "reference", max_half, "LeftOperand");
            controller.setInstructionInputValue(play, max_half, "RightOperand", 2);

            //min / 2 + max / 2
            uint min_half_plus_max_half = controller.addInstruction(play, INSTRUCTION_ID.ADD, new List<uint> { integer, integer, integer });
            controller.linkInstructionData(play, min_half, "result", min_half_plus_max_half, "LeftOperand");
            controller.linkInstructionData(play, max_half, "result", min_half_plus_max_half, "RightOperand");
            
            //result = min / 2 + max / 2
            uint result_calculation = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.linkInstructionData(play, min_half_plus_max_half, "result", result_calculation, "value");

            //result == lastGiven
            uint get_result = controller.addInstruction(play, INSTRUCTION_ID.GETTER, new List<uint> { play_result });
            uint res_eq_last_given = controller.addInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { integer, integer });
            controller.linkInstructionData(play, get_lastGiven, "reference", res_eq_last_given, "LeftOperand");
            controller.linkInstructionData(play, get_result, "reference", res_eq_last_given, "RightOperand");

            //if (result == lastGiven)
            uint if_res_eq_last_given = controller.addInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.linkInstructionData(play, res_eq_last_given, "result", if_res_eq_last_given, "condition");

            //lastResult == MORE
            uint last_result_eq_more = controller.addInstruction(play, INSTRUCTION_ID.EQUAL, new List<uint> { COMPARISON, COMPARISON });
            controller.linkInstructionData(play, get_last_result, "reference", last_result_eq_more, "LeftOperand");
            controller.linkInstructionData(play, split_COMPARISON, "MORE", last_result_eq_more, "RightOperand");

            //if (lastResult == MORE)
            uint if_last_result_eq_more = controller.addInstruction(play, INSTRUCTION_ID.IF, empty);
            controller.linkInstructionData(play, last_result_eq_more, "result", if_last_result_eq_more, "condition");

            //result + 1
            uint result_pp = controller.addInstruction(play, INSTRUCTION_ID.ADD, new List<uint> { integer, integer, integer });
            controller.linkInstructionData(play, get_result, "reference", result_pp, "LeftOperand");
            controller.setInstructionInputValue(play, result_pp, "RightOperand", 1);

            //result = result + 1
            uint set_result_pp = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.linkInstructionData(play, result_pp, "result", set_result_pp, "value");

            //result - 1
            uint result_mm = controller.addInstruction(play, INSTRUCTION_ID.SUB, new List<uint> { integer, integer, integer });
            controller.linkInstructionData(play, get_result, "reference", result_mm, "LeftOperand");
            controller.setInstructionInputValue(play, result_mm, "RightOperand", 1);

            //result = result - 1
            uint set_result_mm = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { play_result });
            controller.linkInstructionData(play, result_mm, "result", set_result_mm, "value");

            //lastGiven = result
            uint set_last_given = controller.addInstruction(play, INSTRUCTION_ID.SETTER, new List<uint> { lastGiven });
            controller.linkInstructionData(play, get_result, "reference", set_last_given, "value");

            /*
             * if (lastResult == More)
             * {
             *      min = lastGiven
             * }
             * else
             * {
             *      if (lastResult == LESS)
             *      {
             *          max = lastGiven
             *      }
             * }
             * 
             * result = min / 2 + max / 2
             * 
             * if (number == lastGiven)
             * {
             *      if (lastResult == MORE)
             *      {
             *          number = number + 1;
             *      }
             *      else
             *      {
             *          number = number - 1;
             *      }
             * }
             * 
             * lastGiven = number
             */
            controller.linkInstructionExecution(play, if_lr_eq_more, (uint)If.ConditionIndexes.OnTrue, set_min);
            controller.linkInstructionExecution(play, if_lr_eq_more, (uint)If.ConditionIndexes.OnFalse, if_lr_eq_less);

            controller.linkInstructionExecution(play, set_min, 0, if_lr_eq_less);

            controller.linkInstructionExecution(play, if_lr_eq_less, (uint)If.ConditionIndexes.OnTrue, set_max);
            controller.linkInstructionExecution(play, if_lr_eq_less, (uint)If.ConditionIndexes.OnFalse, result_calculation);

            controller.linkInstructionExecution(play, set_max, 0, result_calculation);

            controller.linkInstructionExecution(play, result_calculation, 0, if_res_eq_last_given);

            controller.linkInstructionExecution(play, if_res_eq_last_given, (uint)If.ConditionIndexes.OnTrue, if_last_result_eq_more);
            controller.linkInstructionExecution(play, if_res_eq_last_given, (uint)If.ConditionIndexes.OnFalse, set_last_given);

            controller.linkInstructionExecution(play, if_last_result_eq_more, (uint)If.ConditionIndexes.OnTrue, set_result_pp);
            controller.linkInstructionExecution(play, if_last_result_eq_more, (uint)If.ConditionIndexes.OnFalse, set_result_mm);

            controller.linkInstructionExecution(play, set_result_pp, 0, set_last_given);
            controller.linkInstructionExecution(play, set_result_mm, 0, set_last_given);

            controller.setFunctionEntryPoint(play, if_lr_eq_more);

            int mystery_number = 47;

            int i = 0;

            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>
            {
                { "lastResult", controller.getEnumerationValue(COMPARISON, "NONE") }
            };

            Dictionary<string, dynamic> returns;

            do
            {
                returns = controller.callFunction(play, args);

                string toprint = "IA give: " + returns["result"].ToString();

                System.Diagnostics.Debug.WriteLine(toprint);

                if (returns["result"] > mystery_number)
                {
                    args["lastResult"] = controller.getEnumerationValue(COMPARISON, "LESS");
                    System.Diagnostics.Debug.WriteLine("==> It's less");
                }
                else if (returns["result"] < mystery_number)
                {
                    args["lastResult"] = controller.getEnumerationValue(COMPARISON, "MORE");
                    System.Diagnostics.Debug.WriteLine("==> It's more");
                }
                else
                    break;
                ++i;
            } while (returns["result"] != mystery_number && i < 10);

            if (i == 10)
                throw new Exception("Failed to reach mystery number in less that 10 times");
            else
                System.Diagnostics.Debug.Write("AI found the mystery number: " + mystery_number.ToString());
        }

        [TestMethod]
        public void TestCoverage()
        {
            CoreControl.Controler controller = new CoreControl.Controler();
            uint integer = (uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE;
            uint floating = (uint)CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE;

            uint ctx = controller.declare<Context, IContext>(0, "toto", AccessMode.INTERNAL);
            uint fnt = controller.declare<Function>(0, "toto", AccessMode.INTERNAL);
            uint var = controller.declare<Variable>(0, "toto", AccessMode.INTERNAL);
            uint enu = controller.declare<EnumType, DataType>(0, "toto", AccessMode.INTERNAL);
            uint obj = controller.declare<ObjectType, DataType>(0, "tata", AccessMode.INTERNAL);
            uint lst = controller.declare<ListType, DataType>(0, "tutu", AccessMode.INTERNAL);

            //variable

            controller.setVariableType(var, integer);
            controller.setVariableValue(var, 42);

            //enum

            controller.setEnumerationType(enu, floating);
            controller.setEnumerationValue(enu, "TUTU", 43.2);
            Assert.IsTrue(controller.getEnumerationValue(enu, "TUTU") == 43.2);
            controller.removeEnumerationValue(enu, "TUTU");

            //class

            controller.addClassAttribute(obj, "posX", integer, AccessMode.EXTERNAL);
            controller.addClassAttribute(obj, "posY", integer, AccessMode.EXTERNAL);
            controller.renameClassAttribute(obj, "posX", "posZ");
            controller.removeClassAttribute(obj, "posY");

            //uncomment it when object will be implemented

            //controller.addClassMemberFunction(obj, "Unitarize", AccessMode.EXTERNAL);

            //list

            controller.setListType(lst, floating);

            //function

            controller.setVariableType(var, floating);
            uint entry = controller.addInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { var });
            controller.setInstructionInputValue(fnt, entry, "value", 3.14);
            controller.setFunctionEntryPoint(fnt, entry);

            controller.callFunction(fnt, new Dictionary<string, dynamic> { });

            Assert.IsTrue(controller.getVariableValue(var) == 3.14);

            uint val = controller.declare<Variable>(fnt, "value", AccessMode.EXTERNAL);
            controller.setFunctionParameter(fnt, "value");
            controller.setVariableType(val, floating);

            uint get_value = controller.addInstruction(fnt, INSTRUCTION_ID.GETTER, new List<uint> { val });
            controller.linkInstructionData(fnt, get_value, "reference", entry, "value");

            controller.callFunction(fnt, new Dictionary<string, dynamic> { { "value", 42.3 } });

            Assert.IsTrue(controller.getVariableValue(var) == 42.3);

            uint res = controller.declare<Variable>(fnt, "res", AccessMode.EXTERNAL);
            controller.setFunctionReturn(fnt, "res");
            controller.setVariableType(res, floating);

            controller.removeFunctionInstruction(fnt, entry);
            entry = controller.addInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { res });
            controller.setFunctionEntryPoint(fnt, entry);

            controller.linkInstructionData(fnt, get_value, "reference", entry, "value");

            controller.callFunction(fnt, new Dictionary<string, dynamic> { { "value", 56.3 } });

            Assert.IsTrue(controller.getVariableValue(res) == 56.3);

            controller.unlinkInstructionInput(fnt, entry, "value");
            controller.setInstructionInputValue(fnt, entry, "value", 71.2);

            controller.callFunction(fnt, new Dictionary<string, dynamic> { { "value", 31.2 } });
            
            Assert.IsTrue(controller.getVariableValue(res) == 71.2);

            uint new_set = controller.addInstruction(fnt, INSTRUCTION_ID.SETTER, new List<uint> { val });
            controller.linkInstructionData(fnt, get_value, "reference", new_set, "value");
            controller.linkInstructionExecution(fnt, entry, 0, new_set);

            controller.callFunction(fnt, new Dictionary<string, dynamic> { { "value", 32.2 } });

            Assert.IsTrue(controller.getVariableValue(val) == 32.2);

            controller.unlinkInstructionFlow(fnt, entry, 0);

            controller.callFunction(fnt, new Dictionary<string, dynamic> { { "value", 32.2 } });

            Assert.IsTrue(controller.getVariableValue(res) == 71.2);

            controller.changeVisibility<IContext>(0, "toto", AccessMode.INTERNAL);
            controller.changeVisibility<Variable>(0, "toto", AccessMode.INTERNAL);
            controller.changeVisibility<Function>(0, "toto", AccessMode.INTERNAL);
            controller.changeVisibility<DataType>(0, "toto", AccessMode.INTERNAL);
            controller.changeVisibility<DataType>(0, "tata", AccessMode.INTERNAL);
            controller.changeVisibility<DataType>(0, "tutu", AccessMode.INTERNAL);

            uint cnt = controller.declare<Context, IContext>(0, "Container", AccessMode.EXTERNAL);
            controller.move<IContext>(0, cnt, "toto");
            controller.move<Variable>(0, cnt, "toto");
            controller.move<Function>(0, cnt, "toto");
            controller.move<DataType>(0, cnt, "toto");
            controller.move<DataType>(0, cnt, "tata");
            controller.move<DataType>(0, cnt, "tutu");

            controller.rename<IContext>(cnt, "toto", "titi");
            controller.rename<Variable>(cnt, "toto", "titi");
            controller.rename<Function>(cnt, "toto", "titi");
            controller.rename<DataType>(cnt, "toto", "titi");
            controller.rename<DataType>(cnt, "tata", "toto");
            controller.rename<DataType>(cnt, "tutu", "tata");

            controller.remove<IContext>(cnt, "titi");
            controller.remove<Variable>(cnt, "titi");
            controller.remove<Function>(cnt, "titi");
            controller.remove<DataType>(cnt, "titi");
            controller.remove<DataType>(cnt, "toto");
            controller.remove<DataType>(cnt, "tata");
        }
    }
}
