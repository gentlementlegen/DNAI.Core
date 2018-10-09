using CorePackage.Entity;
using CorePackage.Entity.Type;
using CorePackage.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.TestEntities
{
    [TestClass]
    public class TestDict 
    {
        [TestMethod]
        public void EntityUsage()
        {
            Assert.IsTrue(DictType.Instance.IsValueOfType(new Dictionary<string, dynamic>
            {
                { "toto", 42 },
                { "tata", 43.2 },
                { "tutu", "coucou" },
                { "titi", 'q' },
                { "tete", true }
            }));
        }

        [TestMethod]
        public void TestNode()
        {
            var func = new Function();
            var setValueAtKey = new SetValueAtKey();
            var hasKey = new HasKey();
            var dictVar = new Variable(DictType.Instance, new Dictionary<string, dynamic>
            {
                { "tata", "tutu" }
            });
            var boolVar = new Variable(Scalar.Boolean, false);

            uint setValueAtID = func.addInstruction(setValueAtKey);
            uint getDictID = func.addInstruction(new Getter(dictVar));
            uint hasKeyID = func.addInstruction(hasKey);
            uint setBoolVarID = func.addInstruction(new Setter(boolVar));

            func.LinkInstructionData(getDictID, "reference", setValueAtID, "reference");
            func.setEntryPoint(setValueAtID);
            setValueAtKey.SetInputValue("key", "toto");
            setValueAtKey.SetInputValue("value", 3.42);

            hasKey.SetInputValue("key", "toto");
            func.LinkInstructionData(getDictID, "reference", hasKeyID, "reference");
            func.LinkInstructionData(hasKeyID, "result", setBoolVarID, "value");
            func.LinkInstructionExecution(setValueAtID, 0, setBoolVarID);
            
            Assert.IsFalse(dictVar.Value.ContainsKey("toto"));

            func.Call();

            Assert.IsTrue(dictVar.Value.ContainsKey("toto"));
            Assert.IsTrue(dictVar.Value["toto"] == 3.42);
            Assert.IsTrue(boolVar.Value);
        }
    }
}
