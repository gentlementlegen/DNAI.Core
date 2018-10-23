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
            SET_VALUE_AT,
            RETURN,
            HAS_KEY,
            SET_VALUE_AT_KEY,
            REMOVE_VALUE_AT_KEY,
            MACHINE_LEARNING_RUNNER,
            RANDOM,
            CAST
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
            { INSTRUCTION_ID.SET_VALUE_AT, 1 },
            { INSTRUCTION_ID.RETURN, 0 },
            { INSTRUCTION_ID.HAS_KEY, 0 },
            { INSTRUCTION_ID.SET_VALUE_AT_KEY, 0 },
            { INSTRUCTION_ID.REMOVE_VALUE_AT_KEY, 0 },
            { INSTRUCTION_ID.MACHINE_LEARNING_RUNNER, 0 },
            { INSTRUCTION_ID.RANDOM, 0 },
            { INSTRUCTION_ID.CAST, 1 }
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
            { INSTRUCTION_ID.SET_VALUE_AT,      (List<IDefinition> args) => new SetValueAt((DataType)args[0]) },
            { INSTRUCTION_ID.RETURN,            (List<IDefinition> args) => new Return() },
            { INSTRUCTION_ID.HAS_KEY,           (List<IDefinition> args) => new HasKey() },
            { INSTRUCTION_ID.SET_VALUE_AT_KEY,  (List<IDefinition> args) => new SetValueAtKey() },
            { INSTRUCTION_ID.REMOVE_VALUE_AT_KEY,       (List<IDefinition> args) => new RemoveValueAtKey() },
            { INSTRUCTION_ID.MACHINE_LEARNING_RUNNER,   (List<IDefinition> args) => new MachineLearningRunner() },
            { INSTRUCTION_ID.RANDOM,            (List<IDefinition> args) => new CorePackage.Execution.Random() },
            { INSTRUCTION_ID.CAST,              (List<IDefinition> args) => new Cast((DataType)args[0]) }
        };

        /// <summary>
        /// Dictionnary that associates the type of the instruction to its identifier
        /// </summary>
        private static readonly Dictionary<System.Type, INSTRUCTION_ID> types = new Dictionary<Type, INSTRUCTION_ID>
        {
            { typeof(And), INSTRUCTION_ID.AND },
            { typeof(Or), INSTRUCTION_ID.OR },
            { typeof(Different), INSTRUCTION_ID.DIFFERENT },
            { typeof(Equal), INSTRUCTION_ID.EQUAL },
            { typeof(Greater), INSTRUCTION_ID.GREATER },
            { typeof(GreaterEqual), INSTRUCTION_ID.GREATER_EQUAL },
            { typeof(Less), INSTRUCTION_ID.LOWER },
            { typeof(LessEqual), INSTRUCTION_ID.LOWER_EQUAL },
            { typeof(Access), INSTRUCTION_ID.ACCESS },
            { typeof(BinaryAnd), INSTRUCTION_ID.BINARY_AND },
            { typeof(BinaryOr), INSTRUCTION_ID.BINARY_OR },
            { typeof(Xor), INSTRUCTION_ID.XOR },
            { typeof(Add), INSTRUCTION_ID.ADD },
            { typeof(Substract), INSTRUCTION_ID.SUB },
            { typeof(Divide), INSTRUCTION_ID.DIV },
            { typeof(Multiplicate), INSTRUCTION_ID.MUL },
            { typeof(Modulo), INSTRUCTION_ID.MOD },
            { typeof(LeftShift), INSTRUCTION_ID.LEFT_SHIFT },
            { typeof(RightShift), INSTRUCTION_ID.RIGHT_SHIFT },
            { typeof(BinaryNot), INSTRUCTION_ID.BINARY_NOT },
            { typeof(Not), INSTRUCTION_ID.NOT },
            { typeof(Inverse), INSTRUCTION_ID.INVERSE },
            { typeof(EnumSplitter), INSTRUCTION_ID.ENUM_SPLITTER },
            { typeof(Getter), INSTRUCTION_ID.GETTER },
            { typeof(Setter), INSTRUCTION_ID.SETTER },
            { typeof(FunctionCall), INSTRUCTION_ID.FUNCTION_CALL },
            { typeof(If), INSTRUCTION_ID.IF },
            { typeof(While), INSTRUCTION_ID.WHILE },
            { typeof(Append), INSTRUCTION_ID.APPEND },
            { typeof(Insert), INSTRUCTION_ID.INSERT },
            { typeof(Remove), INSTRUCTION_ID.REMOVE },
            { typeof(RemoveIndex), INSTRUCTION_ID.REMOVE_INDEX },
            { typeof(Size), INSTRUCTION_ID.SIZE },
            { typeof(Foreach), INSTRUCTION_ID.FOREACH },
            { typeof(GetAttributes), INSTRUCTION_ID.GET_ATTRIBUTES },
            { typeof(SetAttribute), INSTRUCTION_ID.SET_ATTRIBUTES },
            { typeof(Break), INSTRUCTION_ID.BREAK },
            { typeof(Continue), INSTRUCTION_ID.CONTINUE },
            { typeof(Clear), INSTRUCTION_ID.CLEAR },
            { typeof(Fill), INSTRUCTION_ID.FILL },
            { typeof(SetValueAt), INSTRUCTION_ID.SET_VALUE_AT },
            { typeof(Return), INSTRUCTION_ID.RETURN },
            { typeof(HasKey), INSTRUCTION_ID.HAS_KEY },
            { typeof(SetValueAtKey), INSTRUCTION_ID.SET_VALUE_AT_KEY },
            { typeof(RemoveValueAtKey), INSTRUCTION_ID.REMOVE_VALUE_AT_KEY },
            { typeof(MachineLearningRunner), INSTRUCTION_ID.MACHINE_LEARNING_RUNNER },
            { typeof(CorePackage.Execution.Random), INSTRUCTION_ID.RANDOM },
            { typeof(Cast), INSTRUCTION_ID.CAST }
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

            Instruction created = creators[to_create](arguments);

            created.ConstructionList = arguments;

            return created;
        }

        /// <summary>
        /// Returns the instruction id of a given instruction
        /// </summary>
        /// <param name="instruction">Instruction to which find the type</param>
        /// <returns>Type of the instruction</returns>
        public static INSTRUCTION_ID GetInstructionType(Instruction instruction)
        {
            return types[instruction.GetType()];
        }
    }
}