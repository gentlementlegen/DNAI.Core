using System;
using System.Collections.Generic;

namespace CoreControl
{
    class EntityFactory
    {        
        private Dictionary<UInt32, CorePackage.Global.Definition> definitions = new Dictionary<uint, CorePackage.Global.Definition>();

        private UInt32 current_uid = 1;

        

        public EntityFactory()
        {
            //global context is in 0
            definitions[0] = new CorePackage.Entity.Context();
        }

        public UInt32 CurrentID
        {
            get { return current_uid; }
        }

        public T create<T>() where T : CorePackage.Global.Definition
        {
            T toadd = (T)Activator.CreateInstance(typeof(T)); ;

            definitions[current_uid++] = toadd;
            return toadd;
        }

        /*public CorePackage.Entity.Variable createVariable()
        {
            return create<CorePackage.Entity.Variable>();
            CorePackage.Entity.Variable toadd = new CorePackage.Entity.Variable(varType);

            definitions[current_uid++] = toadd;
            return toadd;
        }

        public CorePackage.Entity.Function createFunction()
        {
            return create<CorePackage.Entity.Function>();
            /*CorePackage.Entity.Function toadd = new CorePackage.Entity.Function();

            definitions[current_uid++] = toadd;
            return toadd;
        }

        public CorePackage.Entity.Type.EnumType createEnum(CorePackage.Entity.DataType stored = null)
        {

            /*CorePackage.Entity.Type.EnumType toadd;

            if (stored != null)
                toadd = new CorePackage.Entity.Type.EnumType(stored);
            else
                toadd = new CorePackage.Entity.Type.EnumType(CorePackage.Entity.Type.Scalar.Integer);
            definitions[current_uid++] = toadd;
            return toadd;
        }

        public CorePackage.Entity.Type.ObjectType createObject(CorePackage.Entity.Context parent)
        {
            CorePackage.Entity.Type.ObjectType toadd = new CorePackage.Entity.Type.ObjectType(parent);

            definitions[current_uid++] = toadd;
            return toadd;
        }

        public CorePackage.Entity.Type.ListType createList(CorePackage.Entity.DataType stored)
        {
            CorePackage.Entity.Type.ListType toadd = new CorePackage.Entity.Type.ListType(stored);

            definitions[current_uid++] = toadd;
            return toadd;
        }

        public CorePackage.Entity.Context createContext(CorePackage.Entity.Context parent = null)
        {
            return create<CorePackage.Entity.Context>();
            /*CorePackage.Entity.Context toadd = new CorePackage.Entity.Context(parent);

            definitions[current_uid++] = toadd;
            return toadd;
        }*/

        public void remove(UInt32 definition_uid)
        {
            if (!definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.remove : given definition uid hasn't been found");

            definitions.Remove(definition_uid);
        }

        public CorePackage.Global.Definition find(UInt32 definition_uid)
        {
            if (!definitions.ContainsKey(definition_uid))
                throw new KeyNotFoundException("EntityFactory.find : given definition uid hasn't been found");

            return definitions[definition_uid];
        }
    }
}
