using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CorePackage.Entity;
using CorePackage.Entity.Type;
using CorePackage.Global;
using static CoreControl.EntityFactory;

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
            
            factory.Create<Context>();
            uint ctx = factory.LastID;
            Assert.IsTrue(ctx == 6 && factory.CurrentID == 7);

            factory.Create<Function>();
            uint fnt = factory.LastID;
            Assert.IsTrue(fnt == 7 && factory.CurrentID == 8);

            factory.Create<Variable>();
            uint var = factory.LastID;
            Assert.IsTrue(var == 8 && factory.CurrentID == 9);

            factory.Create<EnumType>();
            uint enu = factory.LastID;
            Assert.IsTrue(enu == 9 && factory.CurrentID == 10);

            factory.Create<ListType>();
            uint lst = factory.LastID;
            Assert.IsTrue(lst == 10 && factory.CurrentID == 11);

            factory.Create<ObjectType>();
            uint obj = factory.LastID;
            Assert.IsTrue(obj == 11 && factory.CurrentID == 12);

            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.GLOBAL_CTX).GetType() == typeof(Context));
            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.BOOLEAN_TYPE) == Scalar.Boolean);
            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE) == Scalar.Integer);
            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE) == Scalar.Floating);
            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.CHARACTER_TYPE) == Scalar.Character);
            Assert.IsTrue(factory.Find(CoreControl.EntityFactory.BASE_ID.STRING_TYPE) == Scalar.String);

            Assert.IsTrue(factory.Find(ctx).GetType() == typeof(Context));
            Assert.IsTrue(factory.Find(fnt).GetType() == typeof(Function));
            Assert.IsTrue(factory.Find(var).GetType() == typeof(Variable));
            Assert.IsTrue(factory.Find(enu).GetType() == typeof(EnumType));
            Assert.IsTrue(factory.Find(lst).GetType() == typeof(ListType));
            Assert.IsTrue(factory.Find(obj).GetType() == typeof(ObjectType));

            factory.RemoveEntity(ctx);
            factory.RemoveEntity(fnt);
            factory.RemoveEntity(var);
            factory.RemoveEntity(enu);
            factory.RemoveEntity(lst);
            factory.RemoveEntity(obj);

            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.GLOBAL_CTX);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.BOOLEAN_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.INTEGER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.FLOATING_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.CHARACTER_TYPE);
            }));
            Assert.IsTrue(did_call_throw(() => {
                factory.RemoveEntity((uint)CoreControl.EntityFactory.BASE_ID.STRING_TYPE);
            }));

            ctx = factory.Declare<Context, IContext>(0, "toto", AccessMode.INTERNAL);
            fnt = factory.Declare<Function>(0, "toto", AccessMode.INTERNAL);
            var = factory.Declare<Variable>(0, "toto", AccessMode.INTERNAL);
            enu = factory.Declare<EnumType, DataType>(0, "toto", AccessMode.INTERNAL);
            obj = factory.Declare<ObjectType, DataType>(0, "tata", AccessMode.INTERNAL);
            lst = factory.Declare<ListType, DataType>(0, "tutu", AccessMode.INTERNAL);

            factory.Rename<IContext>(0, "toto", "titi");
            factory.Rename<Function>(0, "toto", "titi");
            factory.Rename<Variable>(0, "toto", "titi");
            factory.Rename<DataType>(0, "toto", "titi");
            factory.Rename<DataType>(0, "tata", "toto");
            factory.Rename<DataType>(0, "tutu", "tata");

            uint cnt = factory.Declare<Context, IContext>(0, "container", AccessMode.EXTERNAL);

            factory.Move<IContext>(0, cnt, "titi");
            factory.Move<Function>(0, cnt, "titi");
            factory.Move<Variable>(0, cnt, "titi");
            factory.Move<DataType>(0, cnt, "titi");
            factory.Move<DataType>(0, cnt, "toto");
            factory.Move<DataType>(0, cnt, "tata");

            factory.ChangeVisibility<IContext>(cnt, "titi", AccessMode.INTERNAL);
            factory.ChangeVisibility<Function>(cnt, "titi", AccessMode.INTERNAL);
            factory.ChangeVisibility<Variable>(cnt, "titi", AccessMode.INTERNAL);
            factory.ChangeVisibility<DataType>(cnt, "titi", AccessMode.INTERNAL);
            factory.ChangeVisibility<DataType>(cnt, "toto", AccessMode.INTERNAL);
            factory.ChangeVisibility<DataType>(cnt, "tata", AccessMode.INTERNAL);

            factory.Remove<IContext>(cnt, "titi");
            factory.Remove<Function>(cnt, "titi");
            factory.Remove<Variable>(cnt, "titi");
            factory.Remove<DataType>(cnt, "titi");
            factory.Remove<DataType>(cnt, "toto");
            factory.Remove<DataType>(cnt, "tata");
            factory.Remove<IContext>(0, "container");

            ctx = factory.Declare(ENTITY.CONTEXT_D, 0, "toto", VISIBILITY.PRIVATE);
            fnt = factory.Declare(ENTITY.FUNCTION, 0, "toto", VISIBILITY.PRIVATE);
            var = factory.Declare(ENTITY.VARIABLE, 0, "toto", VISIBILITY.PRIVATE);
            enu = factory.Declare(ENTITY.ENUM_TYPE, 0, "toto", VISIBILITY.PRIVATE);
            obj = factory.Declare(ENTITY.OBJECT_TYPE, 0, "tata", VISIBILITY.PRIVATE);
            lst = factory.Declare(ENTITY.LIST_TYPE, 0, "tutu", VISIBILITY.PRIVATE);

            factory.ChangeVisibility(ENTITY.CONTEXT_D, 0, "toto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(ENTITY.VARIABLE, 0, "toto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(ENTITY.FUNCTION, 0, "toto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(ENTITY.DATA_TYPE, 0, "toto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(ENTITY.DATA_TYPE, 0, "tata", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(ENTITY.DATA_TYPE, 0, "tutu", VISIBILITY.PUBLIC);

            cnt = factory.Declare(ENTITY.CONTEXT_D, 0, "Container", VISIBILITY.PUBLIC);
            factory.Move(ENTITY.CONTEXT_D, 0, cnt, "toto");
            factory.Move(ENTITY.VARIABLE, 0, cnt, "toto");
            factory.Move(ENTITY.FUNCTION, 0, cnt, "toto");
            factory.Move(ENTITY.DATA_TYPE, 0, cnt, "toto");
            factory.Move(ENTITY.DATA_TYPE, 0, cnt, "tata");
            factory.Move(ENTITY.DATA_TYPE, 0, cnt, "tutu");

            factory.Rename(ENTITY.CONTEXT_D, cnt, "toto", "titi");
            factory.Rename(ENTITY.VARIABLE, cnt, "toto", "titi");
            factory.Rename(ENTITY.FUNCTION, cnt, "toto", "titi");
            factory.Rename(ENTITY.DATA_TYPE, cnt, "toto", "titi");
            factory.Rename(ENTITY.DATA_TYPE, cnt, "tata", "toto");
            factory.Rename(ENTITY.DATA_TYPE, cnt, "tutu", "tata");

            factory.Remove(ENTITY.CONTEXT_D, cnt, "titi");
            factory.Remove(ENTITY.VARIABLE, cnt, "titi");
            factory.Remove(ENTITY.FUNCTION, cnt, "titi");
            factory.Remove(ENTITY.DATA_TYPE, cnt, "titi");
            factory.Remove(ENTITY.DATA_TYPE, cnt, "toto");
            factory.Remove(ENTITY.DATA_TYPE, cnt, "tata");
        }
    }
}
