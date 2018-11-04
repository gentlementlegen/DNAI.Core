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
            PREDICT,
            RANDOM,
            CAST,
            GET_SHAPE,
            GET_INDEX_VALUE,
            RESIZE
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
            { INSTRUCTION_ID.PREDICT, 0 },
            { INSTRUCTION_ID.RANDOM, 0 },
            { INSTRUCTION_ID.CAST, 1 },
            { INSTRUCTION_ID.GET_SHAPE, 0 },
            { INSTRUCTION_ID.GET_INDEX_VALUE, 0 },
            { INSTRUCTION_ID.RESIZE, 0 }
        };

        /// <summary>
        /// Dictionary that associates an instruction to its creator delegate
        /// </summary>
        private static readonly Dictionary<INSTRUCTION_ID, Func<List<IDefinition>, Instruction>> creators = new Dictionary<INSTRUCTION_ID, Func<List<IDefinition>, Instruction>>
        {
            { INSTRUCTION_ID.AND,                   args => new And() },
            { INSTRUCTION_ID.OR,                    args => new Or() },
            { INSTRUCTION_ID.DIFFERENT,             args => new Different((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.EQUAL,                 args => new Equal((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.GREATER,               args => new Greater((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.GREATER_EQUAL,         args => new GreaterEqual((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.LOWER,                 args => new Less((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.LOWER_EQUAL,           args => new LessEqual((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.ACCESS,                args => new Access((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_AND,            args => new BinaryAnd((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_OR,             args => new BinaryOr((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.XOR,                   args => new Xor((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.ADD,                   args => new Add((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.SUB,                   args => new Substract((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.DIV,                   args => new Divide((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.MUL,                   args => new Multiplicate((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.MOD,                   args => new Modulo((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.LEFT_SHIFT,            args => new LeftShift((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.RIGHT_SHIFT,           args => new RightShift((DataType)args[0], (DataType)args[1], (DataType)args[2]) },
            { INSTRUCTION_ID.BINARY_NOT,            args => new BinaryNot((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.NOT,                   args => new Not((DataType)args[0]) },
            { INSTRUCTION_ID.INVERSE,               args => new Inverse((DataType)args[0], (DataType)args[1]) },
            { INSTRUCTION_ID.ENUM_SPLITTER,         args => new EnumSplitter((EnumType)args[0]) },
            { INSTRUCTION_ID.GETTER,                args => new Getter((Variable)args[0]) },
            { INSTRUCTION_ID.SETTER,                args => new Setter((Variable)args[0]) },
            { INSTRUCTION_ID.FUNCTION_CALL,         args => new FunctionCall((Function)args[0]) },
            { INSTRUCTION_ID.IF,                    args => new If() },
            { INSTRUCTION_ID.WHILE,                 args => new While() },
            { INSTRUCTION_ID.APPEND,                args => new Append((DataType)args[0]) },
            { INSTRUCTION_ID.INSERT,                args => new Insert((DataType)args[0]) },
            { INSTRUCTION_ID.REMOVE,                args => new Remove((DataType)args[0]) },
            { INSTRUCTION_ID.REMOVE_INDEX,          args => new RemoveIndex((DataType)args[0]) },
            { INSTRUCTION_ID.SIZE,                  args => new Size((DataType)args[0]) },
            { INSTRUCTION_ID.FOREACH,               args => new Foreach((DataType)args[0]) },
            { INSTRUCTION_ID.GET_ATTRIBUTES,        args => new GetAttributes((ObjectType)(args[0])) },
            { INSTRUCTION_ID.SET_ATTRIBUTES,        args => new SetAttribute((ObjectType)args[0]) },
            { INSTRUCTION_ID.BREAK,                 args => new Break() },
            { INSTRUCTION_ID.CONTINUE,              args => new Continue() },
            { INSTRUCTION_ID.CLEAR,                 args => new Clear((DataType)args[0]) },
            { INSTRUCTION_ID.FILL,                  args => new Fill((DataType)args[0]) },
            { INSTRUCTION_ID.SET_VALUE_AT,          args => new SetValueAt((DataType)args[0]) },
            { INSTRUCTION_ID.RETURN,                args => new Return() },
            { INSTRUCTION_ID.HAS_KEY,               args => new HasKey() },
            { INSTRUCTION_ID.SET_VALUE_AT_KEY,      args => new SetValueAtKey() },
            { INSTRUCTION_ID.REMOVE_VALUE_AT_KEY,   args => new RemoveValueAtKey() },
            { INSTRUCTION_ID.PREDICT,               args => new Predict() },
            { INSTRUCTION_ID.RANDOM,                args => new CorePackage.Execution.Random() },
            { INSTRUCTION_ID.CAST,                  args => new Cast((DataType)args[0]) },
            { INSTRUCTION_ID.GET_SHAPE,             args => new GetShape() },
            { INSTRUCTION_ID.GET_INDEX_VALUE,       args => new GetIndexValue() },
            { INSTRUCTION_ID.RESIZE,                args => new Resize() }
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
            { typeof(Predict), INSTRUCTION_ID.PREDICT },
            { typeof(CorePackage.Execution.Random), INSTRUCTION_ID.RANDOM },
            { typeof(Cast), INSTRUCTION_ID.CAST },
            { typeof(GetShape), INSTRUCTION_ID.GET_SHAPE },
            { typeof(GetIndexValue), INSTRUCTION_ID.GET_INDEX_VALUE },
            { typeof(Resize), INSTRUCTION_ID.RESIZE }
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