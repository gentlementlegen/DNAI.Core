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

            ctx = factory.Declare<Context>(0, "Ctoto", AccessMode.INTERNAL);
            fnt = factory.Declare<Function>(0, "Ftoto", AccessMode.INTERNAL);
            var = factory.Declare<Variable>(0, "Vtoto", AccessMode.INTERNAL);
            enu = factory.Declare<EnumType>(0, "Etoto", AccessMode.INTERNAL);
            obj = factory.Declare<ObjectType>(0, "tata", AccessMode.INTERNAL);
            lst = factory.Declare<ListType>(0, "tutu", AccessMode.INTERNAL);

            factory.Rename(0, "Ctoto", "Ctiti");
            factory.Rename(0, "Ftoto", "Ftiti");
            factory.Rename(0, "Vtoto", "Vtiti");
            factory.Rename(0, "Etoto", "Etiti");
            factory.Rename(0, "tata", "toto");
            factory.Rename(0, "tutu", "tata");

            uint cnt = factory.Declare<Context>(0, "container", AccessMode.EXTERNAL);

            factory.Move(0, cnt, "Ctiti");
            factory.Move(0, cnt, "Ftiti");
            factory.Move(0, cnt, "Vtiti");
            factory.Move(0, cnt, "Etiti");
            factory.Move(0, cnt, "toto");
            factory.Move(0, cnt, "tata");

            factory.ChangeVisibility(cnt, "Ctiti", VISIBILITY.PRIVATE);
            factory.ChangeVisibility(cnt, "Ftiti", VISIBILITY.PRIVATE);
            factory.ChangeVisibility(cnt, "Vtiti", VISIBILITY.PRIVATE);
            factory.ChangeVisibility(cnt, "Etiti", VISIBILITY.PRIVATE);
            factory.ChangeVisibility(cnt, "toto", VISIBILITY.PRIVATE);
            factory.ChangeVisibility(cnt, "tata", VISIBILITY.PRIVATE);

            factory.Remove(cnt, "Ctiti");
            factory.Remove(cnt, "Ftiti");
            factory.Remove(cnt, "Vtiti");
            factory.Remove(cnt, "Etiti");
            factory.Remove(cnt, "toto");
            factory.Remove(cnt, "tata");
            factory.Remove(0, "container");

            ctx = factory.Declare(ENTITY.CONTEXT, 0, "Ctoto", VISIBILITY.PRIVATE);
            fnt = factory.Declare(ENTITY.FUNCTION, 0, "Ftoto", VISIBILITY.PRIVATE);
            var = factory.Declare(ENTITY.VARIABLE, 0, "Vtoto", VISIBILITY.PRIVATE);
            enu = factory.Declare(ENTITY.ENUM_TYPE, 0, "Etoto", VISIBILITY.PRIVATE);
            obj = factory.Declare(ENTITY.OBJECT_TYPE, 0, "tata", VISIBILITY.PRIVATE);
            lst = factory.Declare(ENTITY.LIST_TYPE, 0, "tutu", VISIBILITY.PRIVATE);

            factory.ChangeVisibility(0, "Ctoto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(0, "Ftoto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(0, "Vtoto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(0, "Etoto", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(0, "tata", VISIBILITY.PUBLIC);
            factory.ChangeVisibility(0, "tutu", VISIBILITY.PUBLIC);

            cnt = factory.Declare(ENTITY.CONTEXT, 0, "Container", VISIBILITY.PUBLIC);
            factory.Move(0, cnt, "Ctoto");
            factory.Move(0, cnt, "Ftoto");
            factory.Move(0, cnt, "Vtoto");
            factory.Move(0, cnt, "Etoto");
            factory.Move(0, cnt, "tata");
            factory.Move(0, cnt, "tutu");

            factory.Rename(cnt, "Ctoto", "Ctiti");
            factory.Rename(cnt, "Ftoto", "Ftiti");
            factory.Rename(cnt, "Vtoto", "Vtiti");
            factory.Rename(cnt, "Etoto", "Etiti");
            factory.Rename(cnt, "tata", "toto");
            factory.Rename(cnt, "tutu", "tata");

            factory.Remove(cnt, "Ctiti");
            factory.Remove(cnt, "Ftiti");
            factory.Remove(cnt, "Vtiti");
            factory.Remove(cnt, "Etiti");
            factory.Remove(cnt, "toto");
            factory.Remove(cnt, "tata");
        }
    }
}
