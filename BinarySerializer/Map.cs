using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializer
{
    public class KeyValuePair<_Key, _Value>
    {
        [BinaryFormat]
        public _Key Key { get; set; }

        [BinaryFormat]
        public _Value Value { get; set; }
    }

    public class Map
    {
        [BinaryFormat]
        public IList Data { get; private set; }

        private Type _keyvalueType;

        private Type _keytype;

        private Type _valuetype;

        public Map(Type keytype, Type valueType)
        {
            _keytype = keytype;
            _valuetype = valueType;
            _keyvalueType = typeof(KeyValuePair<int, int>).MakeGenericType(new Type[] { keytype, valueType });
            Data = Activator.CreateInstance(typeof(List<>).MakeGenericType(_keyvalueType)) as IList;
        }

        private bool CheckType(Type key, Type value)
        {
            return key == _keytype && value == _valuetype;
        }

        public void FillFrom<Key, Value>(Dictionary<Key, Value> dat)
        {
            if (!CheckType(typeof(Key), typeof(Value)))
                return;

            foreach (var item in dat)
            {
                Data.Add(Activator.CreateInstance(_keyvalueType, new object[] { item.Key, item.Value }));
            }
        }

        public Dictionary<Key, Value> ConvertTo<Key, Value>()
        {
            if (!CheckType(typeof(Key), typeof(Value)))
                return null;

            Dictionary<Key, Value> toret = new Dictionary<Key, Value>();

            foreach (KeyValuePair<Key, Value> item in Data)
            {
                toret[item.Key] = item.Value;
            }
            return toret;
        }
    }
}
