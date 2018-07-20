using CoreCommand;
using CoreControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCommand
{
    [TestClass]
    public class TestSerialization
    {
        // Check for simple IOC
        [TestMethod]
        public void SerializeParameters()
        {
            //Console.Write(ProtoBuf.Serializer.GetProto<CoreCoreCommand.Command.ProtoData>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            //Controller controller = new Controller();

            /*CommandWatcher watcher = new CommandWatcher();

            watcher.AddCommand(() =>
            {
                return new CoreCommand.Command.Declarator.Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.CONTEXT, Name = "ctx", Visibility = EntityFactory.VISIBILITY.PUBLIC };
            });

            watcher.AddCommand(() =>
            {
                return new CoreCommand.Command.Declarator.Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.ENUM_TYPE, Name = "myEnum", Visibility = EntityFactory.VISIBILITY.PRIVATE };
            });

            watcher.AddCommand(() =>
            {
                return new CoreCommand.Command.Declarator.Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.FUNCTION, Name = "myFx", Visibility = EntityFactory.VISIBILITY.PUBLIC };
            });

            watcher.AddCommand(() =>
            {
                return new CoreCommand.Command.Function.AddInstruction { Name = "And", InstructionId = InstructionFactory.INSTRUCTION_ID.AND, Id = 8 };
            });

            watcher.SerializeCommandsToFile();
            var actions = watcher.DeserializeCommandsFromFile();

            foreach (var a in actions)
            {
                if (a is CoreCommand.Command.Declarator.Declare d)
                {
                    controller.CoreCommand.Command.Declarator.Declare(d.EntityType, d.ContainerID, d.Name, d.Visibility);
                }
                else if (a is CoreCommand.Command.Function.AddInstruction e)
                {
                    controller.CoreCommand.Command.Function.AddInstruction(e.Id, e.InstructionId, e.Arguments);
                }
            }*/

            /*CoreCommand.Command.Variable.SetValue test = new CoreCommand.Command.Variable.SetValue { VariableID = 42, Value = "toto" };
            //CoreCommand.Command.Declarator.Declare test = new CoreCommand.Command.Declarator.Declare { ContainerID = 42, EntityType = ENTITY.CONTEXT, Name = "toto", Visibility = VISIBILITY.PRIVATE };

            System.IO.MemoryStream to_wr = new System.IO.MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(to_wr, test, ProtoBuf.PrefixStyle.Base128);

            to_wr.Position = 0;

            CoreCommand.Command.Variable.SetValue deser = ProtoBuf.Serializer.DeserializeWithLengthPrefix<CoreCommand.Command.Variable.SetValue>(to_wr, ProtoBuf.PrefixStyle.Base128);

            Assert.IsFalse(deser == null);*/

            //Assert.IsTrue(deser.VariableID == 42 && deser.Value == "toto");
        }

        private Reply HandleCommand<Reply, Command>(Command tohandle, CoreCommand.IManager manager)
        {
            MemoryStream inp = new MemoryStream();
            MemoryStream oup = new MemoryStream();

            BinarySerializer.Serializer.Serialize(tohandle, inp);
            inp.Position = 0;
            if (!manager.CallCommand(manager.GetCommandName(typeof(Command)), inp, oup))
            {
                oup.Position = 0;
                throw new Exception("Failed to call command: \"" + BinarySerializer.Serializer.Deserialize<String>(oup) + "\"");
            }
            oup.Position = 0;
            BinarySerializer.Serializer.Deserialize<Command>(oup);
            return BinarySerializer.Serializer.Deserialize<Reply>(oup);
        }

        private void MoreOrLessExecuter(CoreCommand.IManager manager, UInt32 funcID)
        {
            int mystery_number = 47;

            int i = 0;

            CoreCommand.Command.Function.Call command = new CoreCommand.Command.Function.Call
            {
                FuncId = funcID,
                Parameters = new Dictionary<string, string>
                {
                    { "lastResult", "2" }
                }
            };
            CoreCommand.Command.Function.Call.Reply reply;
            int given;

            do
            {
                reply = HandleCommand<CoreCommand.Command.Function.Call.Reply, CoreCommand.Command.Function.Call>(command, manager);

                given = Int32.Parse(reply.Returns["result"]);

                Console.WriteLine("IA said: " + given.ToString());

                if (given > mystery_number)
                {
                    command.Parameters["lastResult"] = "1"; //less
                    Console.WriteLine("==> It's Less");
                }
                else if (given < mystery_number)
                {
                    command.Parameters["lastResult"] = "0"; //more
                    Console.WriteLine("==> It's More");
                }
                ++i;
            } while (given != mystery_number && i < 10);

            Assert.IsTrue(given == mystery_number);

            Console.WriteLine("AI found the mystery number " + mystery_number + " in " + i + " hits");
        }

        [TestMethod]
        public void GenerateMoreOrLess()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();
            MemoryStream commands = new MemoryStream();
            MemoryStream output = new MemoryStream();
            uint integer = (uint)EntityFactory.BASE_ID.INTEGER_TYPE;

            CoreCommand.Command.Declarator.Declare.Reply moreOrLessContext = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.CONTEXT,
                    ContainerID = 0,
                    Name = "moreOrLess",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);

            //int min = 0;
            CoreCommand.Command.Declarator.Declare.Reply minVariable = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "min",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = minVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = minVariable.EntityID,
                    Value = "0"
                }, manager);

            //int max = 100;
            CoreCommand.Command.Declarator.Declare.Reply maxVariable = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "max",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = maxVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = maxVariable.EntityID,
                    Value = "100"
                }, manager);

            //int lastGiven = -1;
            CoreCommand.Command.Declarator.Declare.Reply lastGivenVariable = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "lastGiven",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = lastGivenVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = lastGivenVariable.EntityID,
                    Value = "-1"
                }, manager);

            /*
             * enum COMPARISON
             * {
             *      MORE = 0,
             *      LESS = 1,
             *      NONE = 2
             * }
             */
            CoreCommand.Command.Declarator.Declare.Reply COMPARISONenum = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.ENUM_TYPE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "COMPARISON",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "MORE",
                    Value = "0"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "LESS",
                    Value = "1"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Enum.SetValue>(
                new CoreCommand.Command.Enum.SetValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "NONE",
                    Value = "2"
                }, manager);

            /*
             * COMPARISON Play(COMPARISON lastResult = NONE);
             */
            CoreCommand.Command.Declarator.Declare.Reply playFunction = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.FUNCTION,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "Play",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            CoreCommand.Command.Declarator.Declare.Reply play_lastResultVariable = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "lastResult",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.SetParameter>(
                new CoreCommand.Command.Function.SetParameter
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "lastResult"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = play_lastResultVariable.EntityID,
                    TypeID = COMPARISONenum.EntityID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetValue>(
                new CoreCommand.Command.Variable.SetValue
                {
                    VariableID = play_lastResultVariable.EntityID,
                    Value = "2"
                }, manager);
            CoreCommand.Command.Declarator.Declare.Reply play_resultVariable = HandleCommand<CoreCommand.Command.Declarator.Declare.Reply, CoreCommand.Command.Declarator.Declare>(
                new CoreCommand.Command.Declarator.Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "result",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.SetReturn>(
                new CoreCommand.Command.Function.SetReturn
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "result"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Variable.SetType>(
                new CoreCommand.Command.Variable.SetType
                {
                    VariableID = play_resultVariable.EntityID,
                    TypeID = integer
                }, manager);

            CoreCommand.Command.Function.AddInstruction.Reply splitCOMPARISON = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ENUM_SPLITTER,
                    Arguments = new List<uint> { COMPARISONenum.EntityID }
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply getLastResult = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_lastResultVariable.EntityID }
                }, manager);

            //if (lastResult == COMPARISON::MORE)
            CoreCommand.Command.Function.AddInstruction.Reply lr_eq_more = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply if_lr_eq_more = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_more.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_more.InstructionID,
                    InputName = "condition"
                }, manager);

            //min = lastGiven
            CoreCommand.Command.Function.AddInstruction.Reply getLastGiven = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply setMin = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = setMin.InstructionID,
                    InputName = "value"
                }, manager);

            //if (lastResult == COMPARISON::LESS)
            CoreCommand.Command.Function.AddInstruction.Reply lr_eq_less = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "LESS",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply if_lr_eq_less = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_less.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_less.InstructionID,
                    InputName = "condition"
                }, manager);

            //max = lastGiven
            CoreCommand.Command.Function.AddInstruction.Reply set_max = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = set_max.InstructionID,
                    InputName = "value"
                }, manager);

            //min / 2
            CoreCommand.Command.Function.AddInstruction.Reply getMin = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply minHalf = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMin.InstructionID,
                    OutputName = "reference",
                    ToId = minHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.SetInputValue>(
                new CoreCommand.Command.Function.Instruction.SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = minHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //max / 2
            CoreCommand.Command.Function.AddInstruction.Reply getMax = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply maxHalf = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMax.InstructionID,
                    OutputName = "reference",
                    ToId = maxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.SetInputValue>(
                new CoreCommand.Command.Function.Instruction.SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = maxHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //min / 2 + max / 2
            CoreCommand.Command.Function.AddInstruction.Reply minHalfPlusMaxHalf = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = maxHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //result = min / 2 + max / 2
            CoreCommand.Command.Function.AddInstruction.Reply setResult = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalfPlusMaxHalf.InstructionID,
                    OutputName = "result",
                    ToId = setResult.InstructionID,
                    InputName = "value"
                }, manager);

            //result == lastGiven
            CoreCommand.Command.Function.AddInstruction.Reply getResult = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            CoreCommand.Command.Function.AddInstruction.Reply resEqLastGiven = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (result == lastGiven)
            CoreCommand.Command.Function.AddInstruction.Reply ifResEqLastGiven = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resEqLastGiven.InstructionID,
                    OutputName = "result",
                    ToId = ifResEqLastGiven.InstructionID,
                    InputName = "condition"
                }, manager);

            //lastResult == MORE
            CoreCommand.Command.Function.AddInstruction.Reply lastResultEqMore = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (lastResult == MORE)
            CoreCommand.Command.Function.AddInstruction.Reply ifLastResultEqMore = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lastResultEqMore.InstructionID,
                    OutputName = "result",
                    ToId = ifLastResultEqMore.InstructionID,
                    InputName = "condition"
                }, manager);

            //result + 1
            CoreCommand.Command.Function.AddInstruction.Reply resultPP = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultPP.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.SetInputValue>(
                new CoreCommand.Command.Function.Instruction.SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultPP.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result + 1
            CoreCommand.Command.Function.AddInstruction.Reply setResultPP = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultPP.InstructionID,
                    OutputName = "result",
                    ToId = setResultPP.InstructionID,
                    InputName = "value"
                }, manager);

            //result - 1
            CoreCommand.Command.Function.AddInstruction.Reply resultMM = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SUB,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultMM.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.SetInputValue>(
                new CoreCommand.Command.Function.Instruction.SetInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultMM.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result - 1
            CoreCommand.Command.Function.AddInstruction.Reply setResultMM = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultMM.InstructionID,
                    OutputName = "result",
                    ToId = setResultMM.InstructionID,
                    InputName = "value"
                }, manager);

            //lastGiven = result
            CoreCommand.Command.Function.AddInstruction.Reply setLastGiven = HandleCommand<CoreCommand.Command.Function.AddInstruction.Reply, CoreCommand.Command.Function.AddInstruction>(
                new CoreCommand.Command.Function.AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkData>(
                new CoreCommand.Command.Function.Instruction.LinkData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = setLastGiven.InstructionID,
                    InputName = "value"
                }, manager);

            /*
             * if (lastResult == MORE)
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
             *          number = number + 1
             *      }
             *      else
             *      {
             *          number = number - 1
             *      }
             * }
             * 
             * lastGiven = number
             */

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setMin.InstructionID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 1, //on false
                    ToId = if_lr_eq_less.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setMin.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 0, //on true
                    ToId = set_max.InstructionID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = set_max.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResult.InstructionID,
                    OutIndex = 0,
                    ToId = ifResEqLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 0, //on true
                    ToId = ifLastResultEqMore.InstructionID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setResultPP.InstructionID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResultMM.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultPP.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);
            HandleCommand<EmptyReply, CoreCommand.Command.Function.Instruction.LinkExecution>(
                new CoreCommand.Command.Function.Instruction.LinkExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultMM.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<EmptyReply, CoreCommand.Command.Function.SetEntryPoint>(
                new CoreCommand.Command.Function.SetEntryPoint
                {
                    FunctionId = playFunction.EntityID,
                    Instruction = if_lr_eq_more.InstructionID
                }, manager);

            MoreOrLessExecuter(manager, playFunction.EntityID);

            HandleCommand<EmptyReply, CoreCommand.Command.Global.Save>(new CoreCommand.Command.Global.Save
            {
                Filename = "moreOrLess.duly"
            }, manager);

            IManager witness = new BinaryManager();

            HandleCommand<EmptyReply, CoreCommand.Command.Global.Load>(new CoreCommand.Command.Global.Load
            {
                Filename = "moreOrLess.duly"
            }, witness);

            MoreOrLessExecuter(witness, playFunction.EntityID);
        }

        public class Pos
        {
            public float X, Y, Z;
        }

        public class PosGraph
        {
            public List<List<int>> links = new List<List<int>>();
            public List<Pos> nodes = new List<Pos>();
        }

        [TestMethod]
        public void TestAstar()
        {
            CoreCommand.BinaryManager manager = new CoreCommand.BinaryManager();

            manager.LoadCommandsFrom("astar.dnai");

            EntityFactory.Entity astarProject = manager.Controller.GetEntitiesOfType(EntityFactory.ENTITY.CONTEXT, 0)[0];
            EntityFactory.Entity posGraphClass = manager.Controller.GetEntitiesOfType(EntityFactory.ENTITY.OBJECT_TYPE, astarProject.Id)[1];
            List<EntityFactory.Entity> funcs = manager.Controller.GetEntitiesOfType(EntityFactory.ENTITY.FUNCTION, posGraphClass.Id);

            EntityFactory.Entity appendNode = null, linkNodes = null, pathfindAstar = null;

            foreach (EntityFactory.Entity curr in funcs)
            {
                if (curr.Name == "appendNode")
                    appendNode = curr;
                else if (curr.Name == "linkNodes")
                    linkNodes = curr;
                else if (curr.Name == "pathFindAStar")
                    pathfindAstar = curr;
            }

            Assert.IsFalse(appendNode == null);
            Assert.IsFalse(linkNodes == null);
            Assert.IsFalse(pathfindAstar == null);

            PosGraph graph = new PosGraph();

            manager.Controller.CallFunction(appendNode.Id, new Dictionary<string, dynamic> { { "node", new Pos { X = 0f, Y = 0f, Z = 0f } }, { "this", graph } });
            manager.Controller.CallFunction(appendNode.Id, new Dictionary<string, dynamic> { { "node", new Pos { X = 1f, Y = 1f, Z = 0f } }, { "this", graph } });

            Assert.IsTrue(graph.nodes.Count == 2);
            Assert.IsTrue(graph.links.Count == 2);

            manager.Controller.CallFunction(linkNodes.Id, new Dictionary<string, dynamic> { { "from", 1 }, { "to", 0 }, { "this", graph }, { "bidirectionnal", false } });

            Assert.IsTrue(graph.links[1].Count == 1);
            Assert.IsTrue(graph.links[1][0] == 0);

            Assert.IsTrue(graph.links[0].Count == 0);

            manager.Controller.CallFunction(appendNode.Id, new Dictionary<string, dynamic> { { "node", new Pos { X = 2f, Y = 2f, Z = 0f } }, { "this", graph } });

            Assert.IsTrue(graph.nodes.Count == 3);
            Assert.IsTrue(graph.links.Count == 3);

            manager.Controller.CallFunction(linkNodes.Id, new Dictionary<string, dynamic> { { "from", 2 }, { "to", 0 }, { "this", graph }, { "bidirectionnal", false } });

            Assert.IsTrue(graph.links[0].Count == 0);

            Assert.IsTrue(graph.links[2].Count == 1);
            Assert.IsTrue(graph.links[2][0] == 0);

            List<int> path = manager.Controller.CallFunction(pathfindAstar.Id, new Dictionary<string, dynamic> { { "from", 2 }, { "to", 1 }, { "this", graph } })["path"];

            Assert.IsTrue(path.Count == 0); //the pathfind fails because the link 0 -> 1 doesn't exists (only 1 -> 0)

            manager.Controller.CallFunction(linkNodes.Id, new Dictionary<string, dynamic> { { "from", 0 }, { "to", 1 }, { "this", graph }, { "bidirectionnal", false } });

            Assert.IsTrue(graph.links[0].Count == 1);
            Assert.IsTrue(graph.links[0][0] == 1);

            Assert.IsTrue(graph.links[1].Count == 1);

            path = (List<int>)manager.Controller.CallFunction(pathfindAstar.Id, new Dictionary<string, dynamic> { { "from", 2 }, { "to", 1 }, { "this", graph } })["path"];

            Assert.IsTrue(path.Count == 3);
            Assert.IsTrue(!path.Except(new List<int> { 2, 0, 1 }).Any());
        }
    }
}
