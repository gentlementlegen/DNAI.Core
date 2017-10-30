using CorePackage.Entity;
using CorePackage.Execution;
using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl
{
    public class InstructionFactory
    {
        public enum INSTRUCTION_ID
        {
            AND,
            OR,
            DIFFERENT,
            EQUAL,
            GREATER,
            GREATER_EQUAL,
            LOWER,
            LOWER_EQUAL,
            ACCESS,
            BINARY_AND,
            BINARY_OR,
            XOR,
            ADD,
            SUB,
            DIV,
            MUL,
            MOD,
            LEFT_SHIFT,
            RIGHT_SHIFT,
            BINARY_NOT,
            NOT,
            INVERSE,
            GETTER,
            SETTER,
            FUNCTION_CALL,
            IF,
            WHILE
        };

        private static Dictionary<INSTRUCTION_ID, uint> number_of_arguments = new Dictionary<INSTRUCTION_ID, uint>
        {
            { INSTRUCTION_ID.AND, 2 },
            { INSTRUCTION_ID.OR, 2 },
            { INSTRUCTION_ID.DIFFERENT, 2 },
            { INSTRUCTION_ID.EQUAL, 2 },
            { INSTRUCTION_ID.GREATER, 2 },
            { INSTRUCTION_ID.GREATER_EQUAL, 2 },
            { INSTRUCTION_ID.LOWER, 2 },
            { INSTRUCTION_ID.LOWER_EQUAL, 2 },
            { INSTRUCTION_ID.ACCESS, 3 },
            { INSTRUCTION_ID.BINARY_AND, 3 },
            { INSTRUCTION_ID.BINARY_OR, 3 },
            { INSTRUCTION_ID.XOR, 3 },
            { INSTRUCTION_ID.ADD, 3 },
            { INSTRUCTION_ID.SUB, 3 },
            { INSTRUCTION_ID.DIV, 3 },
            { INSTRUCTION_ID.MUL, 3 },
            { INSTRUCTION_ID.MOD, 3 },
            { INSTRUCTION_ID.LEFT_SHIFT, 3 },
            { INSTRUCTION_ID.RIGHT_SHIFT, 3 },
            { INSTRUCTION_ID.BINARY_NOT, 2 },
            { INSTRUCTION_ID.NOT, 1 },
            { INSTRUCTION_ID.INVERSE, 2 },
            { INSTRUCTION_ID.GETTER, 1 },
            { INSTRUCTION_ID.SETTER, 1 },
            { INSTRUCTION_ID.FUNCTION_CALL, 1 },
            { INSTRUCTION_ID.IF, 0 },
            { INSTRUCTION_ID.WHILE, 0 }
        };

        private static Dictionary<INSTRUCTION_ID, Func<List<Definition>, Instruction>> creators = new Dictionary<INSTRUCTION_ID, Func<List<CorePackage.Global.Definition>, CorePackage.Execution.Instruction>>
        {
            {
                INSTRUCTION_ID.AND, (List<Definition> arguments) =>
                {
                    return new CorePackage.Execution.Operators.Add(
                        (DataType)arguments.ElementAt(0),
                        (DataType)arguments.ElementAt(1),
                        (DataType)arguments.ElementAt(2));
                }
            }
        };

        public static Instruction create_instruction(INSTRUCTION_ID to_create, List<Definition> arguments)
        {
            if (!number_of_arguments.ContainsKey(to_create) || !creators.ContainsKey(to_create))
                throw new KeyNotFoundException("Given instruction isn't referenced in factory");
            if (arguments.Count() < number_of_arguments[to_create])
                return creators[to_create](arguments);
            throw new InvalidProgramException("Not enought arguments to construct intruction");
        }
    }
}
