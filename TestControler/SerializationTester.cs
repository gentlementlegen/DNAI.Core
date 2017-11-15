using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreControl;
using CoreControl.Command;

namespace TestControler
{
    [TestClass]
    public class SerializationTester
    {
        // Check for simple IOC
        [TestMethod]
        public void SerializeParameters()
        {
            Console.Write(ProtoBuf.Serializer.GetProto<CoreControl.Command.Default>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            Controller controller = new Controller();

            CommandWatcher watcher = new CommandWatcher();

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
                return new AddInstruction { Name = "And", InstructionId = InstructionFactory.INSTRUCTION_ID.AND, Id = 3 };
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
            }
        }
    }
}
