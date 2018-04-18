using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CorePackage.Global;
using CoreControl;
using CorePackage.Execution.Operators;
using CorePackage.Entity.Type;
using CorePackage.Execution;
using CorePackage.Entity;

namespace TestControler
{
    [TestClass]
    public class InstructionFactoryTester
    {
        /// <summary>
        /// Unit test to handle instruction instanciation coverage
        /// </summary>
        [TestMethod]
        public void TestInstructionInstanciation()
        {
            List<IDefinition> empty = new List<IDefinition>();
            List<IDefinition> one_int = new List<IDefinition> { Scalar.Integer };
            List<IDefinition> dbl_int = new List<IDefinition> { Scalar.Integer, Scalar.Integer };
            List<IDefinition> trp_int = new List<IDefinition> { Scalar.Integer, Scalar.Integer, Scalar.Integer };

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.AND, empty).GetType() == typeof(And));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.OR, empty).GetType() == typeof(Or));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.DIFFERENT, dbl_int).GetType() == typeof(Different));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.EQUAL, dbl_int).GetType() == typeof(Equal));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.GREATER, dbl_int).GetType() == typeof(Greater));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.GREATER_EQUAL, dbl_int).GetType() == typeof(GreaterEqual));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.LOWER, dbl_int).GetType() == typeof(Less));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.LOWER_EQUAL, dbl_int).GetType() == typeof(LessEqual));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.ACCESS, trp_int).GetType() == typeof(Access));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.BINARY_AND, trp_int).GetType() == typeof(BinaryAnd));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.BINARY_OR, trp_int).GetType() == typeof(BinaryOr));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.XOR, trp_int).GetType() == typeof(Xor));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.ADD, trp_int).GetType() == typeof(Add));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.SUB, trp_int).GetType() == typeof(Substract));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.DIV, trp_int).GetType() == typeof(Divide));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.MUL, trp_int).GetType() == typeof(Multiplicate));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.MOD, trp_int).GetType() == typeof(Modulo));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.LEFT_SHIFT, trp_int).GetType() == typeof(LeftShift));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.RIGHT_SHIFT, trp_int).GetType() == typeof(RightShift));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.BINARY_NOT, dbl_int).GetType() == typeof(BinaryNot));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.NOT, one_int).GetType() == typeof(Not));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.INVERSE, dbl_int).GetType() == typeof(Inverse));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.ENUM_SPLITTER, new List<IDefinition> { new EnumType() }).GetType() == typeof(EnumSplitter));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.GETTER, new List<IDefinition> { new Variable() }).GetType() == typeof(Getter));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.SETTER, new List<IDefinition> { new Variable() }).GetType() == typeof(Setter));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.FUNCTION_CALL, new List<IDefinition> { new Function() }).GetType() == typeof(FunctionCall));

            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.IF, empty).GetType() == typeof(If));
            Assert.IsTrue(InstructionFactory.CreateInstruction(InstructionFactory.INSTRUCTION_ID.WHILE, empty).GetType() == typeof(While));
        }
    }
}
