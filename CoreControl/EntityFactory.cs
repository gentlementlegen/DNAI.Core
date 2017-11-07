using System;
using System.Collections.Generic;

namespace CoreControl
{
    public class EntityFactory
    {
        public enum BASE_ID : uint
        {
            GLOBAL_CTX = 0,
            BOOLEAN_TYPE = 1,
            INTEGER_TYPE = 2,
            FLOATING_TYPE = 3,
            CHARACTER_TYPE = 4,
            STRING_TYPE = 5
        }

        private Dictionary<UInt32, CorePackage.Global.Definition> definitions = new Dictionary<uint, CorePackage.Global.Definition>();

        private UInt32 current_uid = 6;
        
        public EntityFactory()
        {
            //global context is in 0
            definitions[(uint)BASE_ID.GLOBAL_CTX] = new CorePackage.Entity.Context();
            definitions[(uint)BASE_ID.BOOLEAN_TYPE] = CorePackage.Entity.Type.Scalar.Boolean;
            definitions[(uint)BASE_ID.INTEGER_TYPE] = CorePackage.Entity.Type.Scalar.Integer;
            definitions[(uint)BASE_ID.FLOATING_TYPE] = CorePackage.Entity.Type.Scalar.Floating;
            definitions[(uint)BASE_ID.CHARACTER_TYPE] = CorePackage.Entity.Type.Scalar.Character;
            definitions[(uint)BASE_ID.STRING_TYPE] = CorePackage.Entity.Type.Scalar.String;
        }

        public UInt32 CurrentID
        {
            get { return current_uid; }
        }

        public UInt32 LastID
        {
            get { return CurrentID - 1; }
        }

        public T create<T>() where T : CorePackage.Global.Definition
        {
            T toadd = (T)Activator.CreateInstance(typeof(T)); ;

            definitions[current_uid++] = toadd;
            return toadd;
        }
        
        public void remove(UInt32 definition_uid)
        {
            if (definition_uid <= 5)
                throw new InvalidOperationException("EntityFactory.remove : cannot remove base entities");

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

        public CorePackage.Global.Definition find(BASE_ID definition_uid)
        {
            return find((uint)definition_uid);
        }
    }
}
