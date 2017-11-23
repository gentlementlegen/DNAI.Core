using CoreCommand.Command;
using CoreControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCommand
{
    class TestSerialization
    {
        // Check for simple IOC
        [TestMethod]
        public void SerializeParameters()
        {
            Console.Write(ProtoBuf.Serializer.GetProto<CoreCommand.Command.Default>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            Controller controller = new Controller();

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

            SetVariableValue test = new SetVariableValue { VariableID = 42, Value = "toto" };
            //Declare test = new Declare { ContainerID = 42, EntityType = ENTITY.CONTEXT, Name = "toto", Visibility = VISIBILITY.PRIVATE };

            System.IO.MemoryStream to_wr = new System.IO.MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(to_wr, test, ProtoBuf.PrefixStyle.Base128);

            to_wr.Position = 0;

            SetVariableValue deser = ProtoBuf.Serializer.DeserializeWithLengthPrefix<SetVariableValue>(to_wr, ProtoBuf.PrefixStyle.Base128);

            Assert.IsFalse(deser == null);

            //Assert.IsTrue(deser.VariableID == 42 && deser.Value == "toto");
        }
    }
}
