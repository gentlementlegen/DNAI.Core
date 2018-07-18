using CorePackage.Entity;
using CorePackage.Entity.Type;
using CorePackage.Execution;
using CorePackage.Execution.Operators;
using CorePackage.Global;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreControl
{
    /// <summary>
    /// Class that is used to instanciate instructions
    /// </summary>
    public static class InstructionFactory
    {
        /// <summary>
        /// Enumeration for each possible instruction
        /// </summary>
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
            ENUM_SPLITTER,
            GETTER,
            SETTER,
            FUNCTION_CALL,
            IF,
            WHILE,
            APPEND,
            INSERT,
            REMOVE,
            REMOVE_INDEX,
            SIZE,
            FOREACH,
            GET_ATTRIBUTES,
            SET_ATTRIBUTES,
            BREAK,
            CONTINUE,
            CLEAR,
            FILL,
            SET_VALUE_AT
        };

        /// <summary>
        /// Dictionary that associates an instruction to the number of arguments that takes its constructor
        /// </summary>
        private static readonly Dictionary<INSTRUCTION_ID, uint> number_of_arguments = new Dictionary<INSTRUCTION_ID, uint>
        {
            { INSTRUCTION_ID.AND, 0 },
            { INSTRUCTION_ID.OR, 0 },
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
            { INSTRUCTION_ID.ENUM_SPLITTER, 1 },
            { INSTRUCTION_ID.GETTER, 1 },
            { INSTRUCTION_ID.SETTER, 1 },
            { INSTRUCTION_ID.FUNCTION_CALL, 1 },
            { INSTRUCTION_ID.IF, 0 },
            { INSTRUCTION_ID.WHILE, 0 },
            { INSTRUCTION_ID.APPEND, 1 },
            { INSTRUCTION_ID.INSERT, 1 },
            { INSTRUCTION_ID.REMOVE, 1 },
            { INSTRUCTION_ID.REMOVE_INDEX, 1 },
            { INSTRUCTION_ID.SIZE, 1 },
            { INSTRUCTION_ID.FOREACH, 1 },
            { INSTRUCTION_ID.GET_ATTRIBUTES, 1 },
            { INSTRUCTION_ID.SET_ATTRIBUTES, 1 },
            { INSTRUCTION_ID.BREAK, 0 },
            { INSTRUCTION_ID.CONTINUE, 0 },
            { INSTRUCTION_ID.CLEAR, 1 },
            { INSTRUCTION_ID.FILL, 1 },
            { INSTRUCTION_ID.SET_VALUE_AT, 1 }
        };

        /// <summary>
        /// Dictionary that associates an instruction to its creator delegate
        /// </summary>
        private static readonly Dictionary<INSTRUCTION_ID, Func<List<IDefinition>, Instruction>> creators = new Dictionary<INSTRUCTION_ID, Func<List<IDefinition>, Instruction>>
        {
            { INSTRUCTION_ID.AND,               (List<IDefinition> args) => new And() },
            { INSTRUCTION_ID.OR,                (List<IDefinition> args) => new Or() },
            { INSTRUCTION_ID.DIFFERENT,         (List<IDefinition> args) => new Different((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.EQUAL,             (List<IDefinition> args) => new Equal((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.GREATER,           (List<IDefinition> args) => new Greater((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.GREATER_EQUAL,     (List<IDefinition> args) => new GreaterEqual((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.LOWER,             (List<IDefinition> args) => new Less((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.LOWER_EQUAL,       (List<IDefinition> args) => new LessEqual((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.ACCESS,            (List<IDefinition> args) => new Access((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_AND,        (List<IDefinition> args) => new BinaryAnd((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_OR,         (List<IDefinition> args) => new BinaryOr((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.XOR,               (List<IDefinition> args) => new Xor((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.ADD,               (List<IDefinition> args) => new Add((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.SUB,               (List<IDefinition> args) => new Substract((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.DIV,               (List<IDefinition> args) => new Divide((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.MUL,               (List<IDefinition> args) => new Multiplicate((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.MOD,               (List<IDefinition> args) => new Modulo((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.LEFT_SHIFT,        (List<IDefinition> args) => new LeftShift((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.RIGHT_SHIFT,       (List<IDefinition> args) => new RightShift((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_NOT,        (List<IDefinition> args) => new BinaryNot((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.NOT,               (List<IDefinition> args) => new Not((DataType)args[0]) },
            { INSTRUCTION_ID.INVERSE,           (List<IDefinition> args) => new Inverse((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.ENUM_SPLITTER,     (List<IDefinition> args) => new EnumSplitter((EnumType)args[0]) },
            { INSTRUCTION_ID.GETTER,            (List<IDefinition> args) => new Getter((Variable)args[0]) },
            { INSTRUCTION_ID.SETTER,            (List<IDefinition> args) => new Setter((Variable)args[0]) },
            { INSTRUCTION_ID.FUNCTION_CALL,     (List<IDefinition> args) => new FunctionCall((Function)args[0]) },
            { INSTRUCTION_ID.IF,                (List<IDefinition> args) => new If() },
            { INSTRUCTION_ID.WHILE,             (List<IDefinition> args) => new While() },
            { INSTRUCTION_ID.APPEND,            (List<IDefinition> args) => new Append((DataType)args[0]) },
            { INSTRUCTION_ID.INSERT,            (List<IDefinition> args) => new Insert((DataType)args[0]) },
            { INSTRUCTION_ID.REMOVE,            (List<IDefinition> args) => new Remove((DataType)args[0]) },
            { INSTRUCTION_ID.REMOVE_INDEX,      (List<IDefinition> args) => new RemoveIndex((DataType)args[0]) },
            { INSTRUCTION_ID.SIZE,              (List<IDefinition> args) => new Size((DataType)args[0]) },
            { INSTRUCTION_ID.FOREACH,           (List<IDefinition> args) => new Foreach((DataType)args[0]) },
            { INSTRUCTION_ID.GET_ATTRIBUTES,    (List<IDefinition> args) => new GetAttributes((ObjectType)(args[0])) },
            { INSTRUCTION_ID.SET_ATTRIBUTES,    (List<IDefinition> args) => new SetAttribute((ObjectType)args[0]) },
            { INSTRUCTION_ID.BREAK,             (List<IDefinition> args) => new Break() },
            { INSTRUCTION_ID.CONTINUE,          (List<IDefinition> args) => new Continue() },
            { INSTRUCTION_ID.CLEAR,             (List<IDefinition> args) => new Clear((DataType)args[0]) },
            { INSTRUCTION_ID.FILL,              (List<IDefinition> args) => new Fill((DataType)args[0]) },
            { INSTRUCTION_ID.SET_VALUE_AT,      (List<IDefinition> args) => new SetValueAt((DataType)args[0]) }
        };

        /// <summary>
        /// Create an instruction from an id and a list of arguments
        /// </summary>
        /// <param name="to_create">Type of the instruction to create</param>
        /// <param name="arguments">List of arguments to pass to the instruction at construction</param>
        /// <returns>An instruction of type represented by the give id</returns>
        public static Instruction CreateInstruction(INSTRUCTION_ID to_create, List<IDefinition> arguments)
        {
            if (!number_of_arguments.ContainsKey(to_create) || !creators.ContainsKey(to_create))
                throw new KeyNotFoundException("Given instruction isn't referenced in factory");
            if (arguments.Count < number_of_arguments[to_create])
                throw new InvalidProgramException("Not enought arguments to construct intruction");
            return creators[to_create](arguments);
        }
    }
}