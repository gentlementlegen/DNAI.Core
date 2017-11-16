using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoreControl.Command;
using static CoreControl.EntityFactory;

namespace TestControler
{
    [TestClass]
    public class SerializationTester
    {
        [TestMethod]
        public void SerializeParameters()
        {
            Console.Write(ProtoBuf.Serializer.GetProto<CoreControl.Command.Default>(ProtoBuf.Meta.ProtoSyntax.Proto3));

            SetVariableValue test = new SetVariableValue { VariableID = 42, Value = "toto" };
            //Declare test = new Declare { ContainerID = 42, EntityType = ENTITY.CONTEXT, Name = "toto", Visibility = VISIBILITY.PRIVATE };

            System.IO.MemoryStream to_wr = new System.IO.MemoryStream();

            ProtoBuf.Serializer.SerializeWithLengthPrefix(to_wr, test, ProtoBuf.PrefixStyle.Base128);

            to_wr.Position = 0;

            SetVariableValue deser = ProtoBuf.Serializer.DeserializeWithLengthPrefix<SetVariableValue>(to_wr, ProtoBuf.PrefixStyle.Base128);

            Assert.IsFalse(deser == null);
            
            Assert.IsTrue(deser.VariableID == 42 && deser.Value == "toto" );
        }
    }
}
