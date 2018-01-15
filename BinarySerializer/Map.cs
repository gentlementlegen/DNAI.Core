using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySerializer
{
    public class _KeyValuePair<_Key, _Value>
    {
        [BinaryFormat]
        public _Key Key { get; set; }

        [BinaryFormat]
        public _Value Value { get; set; }
    }

    public class Map
    {
        [BinaryFormat]
        public IList Data { get; set; }

        private Type _keyvalueType;

        private Type _trueKeyValueType;

        private Type _keytype;

        private Type _valuetype;

        public Map(Type keytype, Type valueType)
        {
            _keytype = keytype;
            _valuetype = valueType;
            _keyvalueType = typeof(_KeyValuePair<int, int>).MakeGenericType(new Type[] { keytype, valueType });
            _trueKeyValueType = typeof(KeyValuePair<int, int>).MakeGenericType(new Type[] { _keytype, _valuetype });
            Data = Activator.CreateInstance(typeof(List<>).MakeGenericType(_keyvalueType)) as IList;
        }

        private bool CheckType(Type key, Type value)
        {
            return key == _keytype && value == _valuetype;
        }

        public void FillFrom(IDictionary dat)
        {
            foreach (var item in dat)
            {
                Data.Add(Activator.CreateInstance(_keyvalueType, new object[] {
                    _trueKeyValueType.GetProperties()[0].GetValue(item),
                    _trueKeyValueType.GetProperties()[1].GetValue(item)
                }));
            }
        }

        public IDictionary ToDictionnary()
        {
            /*if (!CheckType(typeof(Key), typeof(Value)))
                return null;*/

            Type dictType = typeof(Dictionary<int, int>).MakeGenericType(new Type[]
            {
                _keytype,
                _valuetype
            });
            IDictionary toret = (IDictionary)Activator.CreateInstance(dictType, new object[] { });

            //Dictionary<Key, Value> toret = new Dictionary<Key, Value>();

            foreach (var item in Data)
            {
                toret[_keyvalueType.GetProperties()[0].GetValue(item)] = _keyvalueType.GetProperties()[1].GetValue(item);
            }
            return toret;
        }
    }
}
