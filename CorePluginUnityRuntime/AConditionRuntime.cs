using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Plugin.Unity.Runtime
{
    public delegate void CallbackFunc();

    /// <summary>
    /// Abstract class for conditions related to Unity GUI callbacks.
    /// Unity does not support Serialization for derived classes,
    /// so this class handles every callbkack that can be used within the editor.
    /// </summary>
    [Serializable]
    public partial class AConditionRuntime /*: ISerializationCallbackReceiver*/
    {
        [SerializeField]
        private int _selectedIdx;

        [SerializeField]
        private List<string> _registeredTypes = new List<string>();

        #region Number Options
        [SerializeField]
        public enum CONDITION_NUMBER { NO_CONDITION, MORE, LESS, EQUAL, DIFFERENT }
        private string[] optionsNumber = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };
        #endregion

        #region String Options
        [SerializeField]
        public enum CONDITION_STRING { NO_CONDITION, EQUAL, DIFFERENT }
        private string[] optionsString = new string[] { "No condition", "Equal to", "Different than" };
        #endregion

        #region Input Serialized Values
        public int InputInt;
        public float InputFloat;
        public string InputString;
        public string InputEnum;
        #endregion

        private readonly Dictionary<string, Func<object, bool>> _evaluateActions = new Dictionary<string, Func<object, bool>>();

        public CallbackFunc Callback;

        [SerializeField]
        private string _currentTypeStr;// = "System.Int64";


        public AConditionRuntime()
        {
            //Debug.Log("Ctor [" + _currentTypeStr + "]");
            //if (!string.IsNullOrEmpty(_currentTypeStr))
            //{
            //    Debug.Log("str is not null or empty");
            //    _currentType = Type.GetType(_currentTypeStr);
            //}

            #region Evaluate Actions
            _evaluateActions.Add(typeof(Int64).ToString(), new Func<object, bool>((obj) =>
            {
                var ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
                //Debug.Log("Evaluate int : " + ConditionNumber + " Input=" + InputInt + " Value=" + obj);
                switch (ConditionNumber)
                {
                    case CONDITION_NUMBER.NO_CONDITION:
                        return true;

                    case CONDITION_NUMBER.MORE:
                        return (Int64)obj > InputInt;

                    case CONDITION_NUMBER.LESS:
                        return (Int64)obj < InputInt;

                    case CONDITION_NUMBER.EQUAL:
                        return (Int64)obj == InputInt;

                    case CONDITION_NUMBER.DIFFERENT:
                        return (Int64)obj != InputInt;
                }
                return false;
            }));
            _evaluateActions.Add(typeof(int).ToString(), new Func<object, bool>((obj) => _evaluateActions[typeof(Int64).ToString()].Invoke(obj)));
            _evaluateActions.Add(typeof(string).ToString(), new Func<object, bool>((obj) =>
            {
                var ConditionString = (CONDITION_STRING)_selectedIdx;
                //Debug.Log("Evaluate string : " + ConditionString + " Input=" + InputString + " Value=" + obj);
                switch (ConditionString)
                {
                    case CONDITION_STRING.NO_CONDITION:
                        return true;

                    case CONDITION_STRING.EQUAL:
                        return (string)obj == InputString;

                    case CONDITION_STRING.DIFFERENT:
                        return (string)obj != InputString;
                }
                return false;
            }));
            _evaluateActions.Add(typeof(float).ToString(), new Func<object, bool>((obj) =>
            {
                var ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
                //Debug.Log("Evaluate float : " + ConditionNumber + " Input=" + InputFloat + " Value=" + obj);
                switch (ConditionNumber)
                {
                    case CONDITION_NUMBER.NO_CONDITION:
                        return true;

                    case CONDITION_NUMBER.MORE:
                        return (float)obj > InputFloat;

                    case CONDITION_NUMBER.LESS:
                        return (float)obj < InputFloat;

                    case CONDITION_NUMBER.EQUAL:
                        return (float)obj == InputFloat;

                    case CONDITION_NUMBER.DIFFERENT:
                        return (float)obj != InputFloat;
                }
                return false;
            }));
            _evaluateActions.Add(typeof(double).ToString(), new Func<object, bool>((obj) => _evaluateActions[typeof(float).ToString()].Invoke(obj)));
            #endregion
        }

        /// <summary>
        /// Registers an enum to the list of handled types.
        /// </summary>
        /// <param name="enumType"></param>
        public void RegisterEnum(string enumType)
        {
            var enumName = enumType.Split(',')[0];
            //Debug.Log("Registering enumeration type =======> " + enumType + " enum name => " + enumName);
            if (!_registeredTypes.Contains(enumType))
            {
                _registeredTypes.Add(enumType);
            }

            if (!_evaluateActions.ContainsKey(enumName))
            {
                _evaluateActions.Add(enumName, new Func<object, bool>((obj) =>
                {
                    var ConditionEnum = (CONDITION_STRING)_selectedIdx;
                    //Debug.Log("Evaluate enum : " + ConditionEnum + " Input=" + InputEnum + " Value=" + obj);
                    switch (ConditionEnum)
                    {
                        case CONDITION_STRING.NO_CONDITION:
                            return true;

                        case CONDITION_STRING.EQUAL:
                            return (string)obj == InputEnum;

                        case CONDITION_STRING.DIFFERENT:
                            return (string)obj != InputEnum;
                    }
                    return false;
                }));
            }
        }

        private T ConvertVariableType<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }

        /// <summary>
        /// Sets the current type used, such as string, int...
        /// </summary>
        /// <param name="type"></param>
        public void SetCurrentType(string type)
        {
            //Debug.Log("Set current type to => " + type);
            _currentTypeStr = type;
            //_currentType = Type.GetType(_currentTypeStr);
            _selectedIdx = 0;
        }

        /// <summary>
        /// Evaluates if the condition is satisfied.
        /// </summary>
        /// <returns></returns>
        public virtual bool Evaluate<T>(T val)
        {
            //if (_currentType == null && !string.IsNullOrEmpty(_currentTypeStr))
            //    _currentType = Type.GetType(_currentTypeStr);
            //Debug.Log("1. Evaluate with type => " + _currentType);
            //Debug.Log("2. Evaluate with type => " + _evaluateActions[_currentTypeStr]);
            return _evaluateActions[_currentTypeStr].Invoke(val);
        }

        /// <summary>
        /// Evaluates a set of conditions, returning true if all of them are satisfied.
        /// </summary>
        /// <param name="cdts"></param>
        /// <returns></returns>
        public static bool EvaluateSet(IEnumerable<AConditionRuntime> cdts)
        {
            throw new NotImplementedException("Evaluate set is not supported.");
            //foreach(var cdt in cdts)
            //{
            //    if (!cdt.Evaluate())
            //        return false;
            //}
            //return true;
        }

        //public void OnBeforeSerialize()
        //{
        //}

        //public void OnAfterDeserialize()
        //{
        //    //Debug.Log("On after desserialize !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + _registeredTypes.Count);
        //    foreach (var item in _registeredTypes)
        //    {
        //        RegisterEnum(item);
        //    }
        //}
    }
}