using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Entity;
using CorePackage.Entity.Type;

namespace TestControler
{
    [TestClass]
    public class EntityFactoryTester
    {
        private bool did_call_throw(Action tocall)
        {
            try
            {
                tocall.DynamicInvoke();
                return false;
            }
            catch
            {
                return true;
            }
        }

        [TestMethod]
        public void TestCreateFindRemove()
        {
            CoreControl.EntityFactory factory = new CoreControl.EntityFactory();
            
            factory.create<Context>();
            uint ctx = factory.LastID;
            Assert.IsTrue(ctx == 6 && factory.CurrentID == 7);

            factory.create<Function>();
            uint fnt = factory.LastID;
            Assert.IsTrue(fnt == 7 && factory.CurrentID == 8);

            factory.create<Variable>();
            uint var = factory.LastID;
            Assert.IsTrue(var == 8 && factory.CurrentID == 9);

            factory.create<EnumType>();
            uint enu = factory.LastID;
            Assert.IsTrue(enu == 9 && factory.CurrentID == 10);

            factory.create<ListType>();
            uint lst = factory.LastID;
            Assert.IsTrue(lst == 10 && factory.CurrentID == 11);

            factory.create<ObjectType>();
            uint obj = factory.LastID;
            Assert.IsTrue(obj == 11 && factory.CurrentID == 12);

            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.GLOBAL_CTX).GetType() == typeof(Context));
            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.BOOLEAN_TYPE) == Scalar.Boolean);
            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE) == Scalar.Integer);
            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE) == Scalar.Floating);
            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.CHARACTER_TYPE) == Scalar.Character);
            Assert.IsTrue(factory.find(CoreControl.EntityFactory.BASE_ID.STRING_TYPE) == Scalar.String);

            Assert.IsTrue(factory.find(ctx).GetType() == typeof(Context));
            Assert.IsTrue(factory.find(fnt).GetType() == typeof(Function));
            Assert.IsTrue(factory.find(var).GetType() == typeof(Variable));
            Assert.IsTrue(factory.find(enu).GetType() == typeof(EnumType));
            Assert.IsTrue(factory.find(lst).GetType() == typeof(ListType));
            Assert.IsTrue(factory.find(obj).GetType() == typeof(ObjectType));

            factory.remove(ctx);
            factory.remove(fnt);
            factory.remove(var);
            factory.remove(enu);
            factory.remove(lst);
            factory.remove(obj);

            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.GLOBAL_CTX);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.BOOLEAN_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.CHARACTER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove((uint)CoreControl.EntityFactory.BASE_ID.STRING_TYPE);
            }));
        }
    }
}
