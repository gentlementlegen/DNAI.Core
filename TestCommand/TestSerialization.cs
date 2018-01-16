using CoreCommand.Command;
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
            //Console.Write(ProtoBuf.Serializer.GetProto<CoreCommand.ProtoData>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            //Controller controller = new Controller();

            /*CommandWatcher watcher = new CommandWatcher();

            watcher.AddCommand(() =>
            {
                return new Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.CONTEXT, Name = "ctx", Visibility = EntityFactory.VISIBILITY.PUBLIC };
            });

            watcher.AddCommand(() =>
            {
                return new Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.ENUM_TYPE, Name = "myEnum", Visibility = EntityFactory.VISIBILITY.PRIVATE };
            });

            watcher.AddCommand(() =>
            {
                return new Declare { ContainerID = 0, EntityType = EntityFactory.ENTITY.FUNCTION, Name = "myFx", Visibility = EntityFactory.VISIBILITY.PUBLIC };
            });

            watcher.AddCommand(() =>
            {
                return new AddInstruction { Name = "And", InstructionId = InstructionFactory.INSTRUCTION_ID.AND, Id = 8 };
            });

            watcher.SerializeCommandsToFile();
            var actions = watcher.DeserializeCommandsFromFile();

            foreach (var a in actions)
            {
                if (a is Declare d)
                {
                    controller.Declare(d.EntityType, d.ContainerID, d.Name, d.Visibility);
                }
                else if (a is AddInstruction e)
                {
                    controller.AddInstruction(e.Id, e.InstructionId, e.Arguments);
                }
            }*/

            /*SetVariableValue test = new SetVariableValue { VariableID = 42, Value = "toto" };
            //Declare test = new Declare { ContainerID = 42, EntityType = ENTITY.CONTEXT, Name = "toto", Visibility = VISIBILITY.PRIVATE };

            System.IO.MemoryStream to_wr = new System.IO.MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(to_wr, test, ProtoBuf.PrefixStyle.Base128);

            to_wr.Position = 0;

            SetVariableValue deser = ProtoBuf.Serializer.DeserializeWithLengthPrefix<SetVariableValue>(to_wr, ProtoBuf.PrefixStyle.Base128);

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
                throw new Exception(BinarySerializer.Serializer.Deserialize<String>(oup));
            }
            oup.Position = 0;
            return BinarySerializer.Serializer.Deserialize<Reply>(oup);
        }

        private void MoreOrLessExecuter(CoreCommand.IManager manager, UInt32 funcID)
        {
            int mystery_number = 47;

            int i = 0;

            CallFunction command = new CallFunction
            {
                FuncId = funcID,
                Parameters = new Dictionary<string, string>
                {
                    { "lastResult", "2" }
                }
            };
            CallFunction.Reply reply;
            int given;

            do
            {
                reply = HandleCommand<CallFunction.Reply, CallFunction>(command, manager);

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

            Declare.Reply moreOrLessContext = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.CONTEXT_D,
                    ContainerID = 0,
                    Name = "moreOrLess",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);

            //int min = 0;
            Declare.Reply minVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "min",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<SetVariableType.Reply, SetVariableType>(
                new SetVariableType
                {
                    VariableID = minVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<SetVariableValue.Reply, SetVariableValue>(
                new SetVariableValue
                {
                    VariableID = minVariable.EntityID,
                    Value = "0"
                }, manager);

            //int max = 100;
            Declare.Reply maxVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "max",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<SetVariableType.Reply, SetVariableType>(
                new SetVariableType
                {
                    VariableID = maxVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<SetVariableValue.Reply, SetVariableValue>(
                new SetVariableValue
                {
                    VariableID = maxVariable.EntityID,
                    Value = "100"
                }, manager);

            //int lastGiven = -1;
            Declare.Reply lastGivenVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "lastGiven",
                    Visibility = EntityFactory.VISIBILITY.PRIVATE
                }, manager);
            HandleCommand<SetVariableType.Reply, SetVariableType>(
                new SetVariableType
                {
                    VariableID = lastGivenVariable.EntityID,
                    TypeID = integer
                }, manager);
            HandleCommand<SetVariableValue.Reply, SetVariableValue>(
                new SetVariableValue
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
            Declare.Reply COMPARISONenum = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.ENUM_TYPE,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "COMPARISON",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<SetEnumerationValue.Reply, SetEnumerationValue>(
                new SetEnumerationValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "MORE",
                    Value = "0"
                }, manager);
            HandleCommand<SetEnumerationValue.Reply, SetEnumerationValue>(
                new SetEnumerationValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "LESS",
                    Value = "1"
                }, manager);
            HandleCommand<SetEnumerationValue.Reply, SetEnumerationValue>(
                new SetEnumerationValue
                {
                    EnumId = COMPARISONenum.EntityID,
                    Name = "NONE",
                    Value = "2"
                }, manager);

            /*
             * COMPARISON Play(COMPARISON lastResult = NONE);
             */
            Declare.Reply playFunction = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.FUNCTION,
                    ContainerID = moreOrLessContext.EntityID,
                    Name = "Play",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            Declare.Reply play_lastResultVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "lastResult",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<SetFunctionParameter.Reply, SetFunctionParameter>(
                new SetFunctionParameter
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "lastResult"
                }, manager);
            HandleCommand<SetVariableType.Reply, SetVariableType>(
                new SetVariableType
                {
                    VariableID = play_lastResultVariable.EntityID,
                    TypeID = COMPARISONenum.EntityID
                }, manager);
            HandleCommand<SetVariableValue.Reply, SetVariableValue>(
                new SetVariableValue
                {
                    VariableID = play_lastResultVariable.EntityID,
                    Value = "2"
                }, manager);
            Declare.Reply play_resultVariable = HandleCommand<Declare.Reply, Declare>(
                new Declare
                {
                    EntityType = EntityFactory.ENTITY.VARIABLE,
                    ContainerID = playFunction.EntityID,
                    Name = "result",
                    Visibility = EntityFactory.VISIBILITY.PUBLIC
                }, manager);
            HandleCommand<SetFunctionReturn.Reply, SetFunctionReturn>(
                new SetFunctionReturn
                {
                    FuncId = playFunction.EntityID,
                    ExternalVarName = "result"
                }, manager);
            HandleCommand<SetVariableType.Reply, SetVariableType>(
                new SetVariableType
                {
                    VariableID = play_resultVariable.EntityID,
                    TypeID = integer
                }, manager);

            AddInstruction.Reply splitCOMPARISON = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ENUM_SPLITTER,
                    Arguments = new List<uint> { COMPARISONenum.EntityID }
                }, manager);
            AddInstruction.Reply getLastResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_lastResultVariable.EntityID }
                }, manager);

            //if (lastResult == COMPARISON::MORE)
            AddInstruction.Reply lr_eq_more = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_more.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            AddInstruction.Reply if_lr_eq_more = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_more.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_more.InstructionID,
                    InputName = "condition"
                }, manager);

            //min = lastGiven
            AddInstruction.Reply getLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            AddInstruction.Reply setMin = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = setMin.InstructionID,
                    InputName = "value"
                }, manager);

            //if (lastResult == COMPARISON::LESS)
            AddInstruction.Reply lr_eq_less = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "LESS",
                    ToId = lr_eq_less.InstructionID,
                    InputName = "RightOperand"
                }, manager);
            AddInstruction.Reply if_lr_eq_less = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lr_eq_less.InstructionID,
                    OutputName = "result",
                    ToId = if_lr_eq_less.InstructionID,
                    InputName = "condition"
                }, manager);

            //max = lastGiven
            AddInstruction.Reply set_max = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = set_max.InstructionID,
                    InputName = "value"
                }, manager);

            //min / 2
            AddInstruction.Reply getMin = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { minVariable.EntityID }
                }, manager);
            AddInstruction.Reply minHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMin.InstructionID,
                    OutputName = "reference",
                    ToId = minHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<SetInstructionInputValue.Reply, SetInstructionInputValue>(
                new SetInstructionInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = minHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //max / 2
            AddInstruction.Reply getMax = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { maxVariable.EntityID }
                }, manager);
            AddInstruction.Reply maxHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.DIV,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getMax.InstructionID,
                    OutputName = "reference",
                    ToId = maxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<SetInstructionInputValue.Reply, SetInstructionInputValue>(
                new SetInstructionInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = maxHalf.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "2"
                }, manager);

            //min / 2 + max / 2
            AddInstruction.Reply minHalfPlusMaxHalf = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = maxHalf.InstructionID,
                    OutputName = "result",
                    ToId = minHalfPlusMaxHalf.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //result = min / 2 + max / 2
            AddInstruction.Reply setResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = minHalfPlusMaxHalf.InstructionID,
                    OutputName = "result",
                    ToId = setResult.InstructionID,
                    InputName = "value"
                }, manager);

            //result == lastGiven
            AddInstruction.Reply getResult = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.GETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            AddInstruction.Reply resEqLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastGiven.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resEqLastGiven.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (result == lastGiven)
            AddInstruction.Reply ifResEqLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resEqLastGiven.InstructionID,
                    OutputName = "result",
                    ToId = ifResEqLastGiven.InstructionID,
                    InputName = "condition"
                }, manager);

            //lastResult == MORE
            AddInstruction.Reply lastResultEqMore = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.EQUAL,
                    Arguments = new List<uint> { COMPARISONenum.EntityID, COMPARISONenum.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getLastResult.InstructionID,
                    OutputName = "reference",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = splitCOMPARISON.InstructionID,
                    OutputName = "MORE",
                    ToId = lastResultEqMore.InstructionID,
                    InputName = "RightOperand"
                }, manager);

            //if (lastResult == MORE)
            AddInstruction.Reply ifLastResultEqMore = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.IF,
                    Arguments = new List<uint> { }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = lastResultEqMore.InstructionID,
                    OutputName = "result",
                    ToId = ifLastResultEqMore.InstructionID,
                    InputName = "condition"
                }, manager);

            //result + 1
            AddInstruction.Reply resultPP = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.ADD,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultPP.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<SetInstructionInputValue.Reply, SetInstructionInputValue>(
                new SetInstructionInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultPP.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result + 1
            AddInstruction.Reply setResultPP = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultPP.InstructionID,
                    OutputName = "result",
                    ToId = setResultPP.InstructionID,
                    InputName = "value"
                }, manager);

            //result - 1
            AddInstruction.Reply resultMM = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SUB,
                    Arguments = new List<uint> { integer, integer, integer }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = getResult.InstructionID,
                    OutputName = "reference",
                    ToId = resultMM.InstructionID,
                    InputName = "LeftOperand"
                }, manager);
            HandleCommand<SetInstructionInputValue.Reply, SetInstructionInputValue>(
                new SetInstructionInputValue
                {
                    FunctionID = playFunction.EntityID,
                    Instruction = resultMM.InstructionID,
                    InputName = "RightOperand",
                    InputValue = "1"
                }, manager);

            //result = result - 1
            AddInstruction.Reply setResultMM = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { play_resultVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
                {
                    FunctionID = playFunction.EntityID,
                    FromId = resultMM.InstructionID,
                    OutputName = "result",
                    ToId = setResultMM.InstructionID,
                    InputName = "value"
                }, manager);

            //lastGiven = result
            AddInstruction.Reply setLastGiven = HandleCommand<AddInstruction.Reply, AddInstruction>(
                new AddInstruction
                {
                    FunctionID = playFunction.EntityID,
                    ToCreate = InstructionFactory.INSTRUCTION_ID.SETTER,
                    Arguments = new List<uint> { lastGivenVariable.EntityID }
                }, manager);
            HandleCommand<LinkInstructionData.Reply, LinkInstructionData>(
                new LinkInstructionData
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

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setMin.InstructionID
                }, manager);
            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_more.InstructionID,
                    OutIndex = 1, //on false
                    ToId = if_lr_eq_less.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setMin.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 0, //on true
                    ToId = set_max.InstructionID
                }, manager);
            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = if_lr_eq_less.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = set_max.InstructionID,
                    OutIndex = 0,
                    ToId = setResult.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResult.InstructionID,
                    OutIndex = 0,
                    ToId = ifResEqLastGiven.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 0, //on true
                    ToId = ifLastResultEqMore.InstructionID
                }, manager);
            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifResEqLastGiven.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 0, //on true
                    ToId = setResultPP.InstructionID
                }, manager);
            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = ifLastResultEqMore.InstructionID,
                    OutIndex = 1, //on false
                    ToId = setResultMM.InstructionID
                }, manager);

            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultPP.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);
            HandleCommand<LinkInstructionExecution.Reply, LinkInstructionExecution>(
                new LinkInstructionExecution
                {
                    FunctionID = playFunction.EntityID,
                    FromId = setResultMM.InstructionID,
                    OutIndex = 0,
                    ToId = setLastGiven.InstructionID
                }, manager);

            HandleCommand<SetFunctionEntryPoint.Reply, SetFunctionEntryPoint>(
                new SetFunctionEntryPoint
                {
                    FunctionId = playFunction.EntityID,
                    Instruction = if_lr_eq_more.InstructionID
                }, manager);

            MoreOrLessExecuter(manager, playFunction.EntityID);

            HandleCommand<SerializeTo.Reply, SerializeTo>(new SerializeTo
            {
                Filename = "moreOrLess.duly"
            }, manager);

            CoreCommand.IManager witness = new CoreCommand.BinaryManager();

            HandleCommand<LoadFrom.Reply, LoadFrom>(new LoadFrom
            {
                Filename = "moreOrLess.duly"
            }, witness);

            MoreOrLessExecuter(witness, playFunction.EntityID);
        }
    }
}
