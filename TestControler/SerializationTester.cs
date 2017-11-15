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
            uint toto = 4;

            Console.Write(ProtoBuf.Serializer.GetProto<CoreControl.Command.Default>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            Controller controller = new Controller();

            CommandWatcher watcher = new CommandWatcher();

            watcher.AddCommand(() =>
            {
                return new Declare { ContainerID = 1, EntityType = EntityFactory.ENTITY.CONTEXT, Name = "ctx", Visibility = EntityFactory.VISIBILITY.PUBLIC };
            });

            watcher.AddCommand(() =>
            {
                return new Declare { ContainerID = toto, EntityType = EntityFactory.ENTITY.ENUM_TYPE, Name = "myEnum", Visibility = EntityFactory.VISIBILITY.PRIVATE };
            });

            toto = 4444;
            watcher.SerializeCommandsToFile();
            var actions = watcher.DeserializeCommandsFromFile();
        }
    }
}
