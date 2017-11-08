using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Entity;
using CorePackage.Entity.Type;
using CorePackage.Global;

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

        /// <summary>
        /// Unit test to handle EntityFactory coverage
        /// </summary>
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

            factory.remove_entity(ctx);
            factory.remove_entity(fnt);
            factory.remove_entity(var);
            factory.remove_entity(enu);
            factory.remove_entity(lst);
            factory.remove_entity(obj);

            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.GLOBAL_CTX);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.BOOLEAN_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.CHARACTER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.remove_entity((uint)CoreControl.EntityFactory.BASE_ID.STRING_TYPE);
            }));

            ctx = factory.declare<Context, IContext>(0, "toto", AccessMode.INTERNAL);
            fnt = factory.declare<Function>(0, "toto", AccessMode.INTERNAL);
            var = factory.declare<Variable>(0, "toto", AccessMode.INTERNAL);
            enu = factory.declare<EnumType, DataType>(0, "toto", AccessMode.INTERNAL);
            obj = factory.declare<ObjectType, DataType>(0, "tata", AccessMode.INTERNAL);
            lst = factory.declare<ListType, DataType>(0, "tutu", AccessMode.INTERNAL);

            factory.rename<IContext>(0, "toto", "titi");
            factory.rename<Function>(0, "toto", "titi");
            factory.rename<Variable>(0, "toto", "titi");
            factory.rename<DataType>(0, "toto", "titi");
            factory.rename<DataType>(0, "tata", "toto");
            factory.rename<DataType>(0, "tutu", "tata");

            uint cnt = factory.declare<Context, IContext>(0, "container", AccessMode.EXTERNAL);

            factory.move<IContext>(0, cnt, "titi");
            factory.move<Function>(0, cnt, "titi");
            factory.move<Variable>(0, cnt, "titi");
            factory.move<DataType>(0, cnt, "titi");
            factory.move<DataType>(0, cnt, "toto");
            factory.move<DataType>(0, cnt, "tata");

            factory.changeVisibility<IContext>(cnt, "titi", AccessMode.INTERNAL);
            factory.changeVisibility<Function>(cnt, "titi", AccessMode.INTERNAL);
            factory.changeVisibility<Variable>(cnt, "titi", AccessMode.INTERNAL);
            factory.changeVisibility<DataType>(cnt, "titi", AccessMode.INTERNAL);
            factory.changeVisibility<DataType>(cnt, "toto", AccessMode.INTERNAL);
            factory.changeVisibility<DataType>(cnt, "tata", AccessMode.INTERNAL);

            factory.remove<IContext>(cnt, "titi");
            factory.remove<Function>(cnt, "titi");
            factory.remove<Variable>(cnt, "titi");
            factory.remove<DataType>(cnt, "titi");
            factory.remove<DataType>(cnt, "toto");
            factory.remove<DataType>(cnt, "tata");
        }
    }
}
