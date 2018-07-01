using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Conditions
{
    public delegate void CallbackFunc();

    /// <summary>
    /// Callback for drawing actions in the inspector.
    /// </summary>
    /// <param name="rect">The last rect drawned by the inspector.</param>
    /// <param name="selectedIdx">The current selected index.</param>
    /// <returns></returns>
    public delegate float DrawingAction(Rect rect, out int selectedIdx);

    /// <summary>
    /// Abstract class for conditions related to Unity GUI callbacks.
    /// Unity does not support Serialization for derived classes,
    /// so this class handles every time that can be used within the editor.
    /// </summary>
    [Serializable]
    public partial class ACondition : ISerializationCallbackReceiver
    {
        private static readonly Dictionary<Type, Type> _matchingTypes = new Dictionary<Type, Type>
        {
            { typeof(int), typeof(ConditionEvaluator) },
        };

        [SerializeField]
        private List<ConditionEvaluator> _matchingInstanceTypes = new List<ConditionEvaluator>
        {
            { new ConditionEvaluator() },
        };

        [SerializeField]
        private int _selectedIdx;

        [SerializeField]
        private List<string> _registeredTypes = new List<string>();

        #region Number Options
        [SerializeField]
        public enum CONDITION_NUMBER { NO_CONDITION, MORE, LESS, EQUAL, DIFFERENT }
        private string[] optionsNumber = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };
        [SerializeField]
        private ConditionInput<int> refOutputInt;
        #endregion

        #region String Options
        [SerializeField]
        public enum CONDITION_STRING { NO_CONDITION, EQUAL, DIFFERENT }
        private string[] optionsString = new string[] { "No condition", "Equal to", "Different than" };
        [SerializeField]
        private ConditionInput<string> refOutputString;
        #endregion

        #region Input Serialized Values
        public int InputInt;
        public float InputFloat;
        public string InputString;
        public string InputEnum;
        #endregion

        /// <summary>
        /// List of drawing actions for given types.
        /// </summary>
        private readonly Dictionary<string, DrawingAction> _drawingActions = new Dictionary<string, DrawingAction>();
        private readonly Dictionary<string, Func<object, bool>> _evaluateActions = new Dictionary<string, Func<object, bool>>();

        public CallbackFunc Callback;

        //public Type CurrentType => _currentType;

        [SerializeField]
        private string _currentTypeStr;// = "System.Int64";
        //private Type _currentType;// = typeof(int);

        private ConditionEvaluator _currentEvaluator;

        public ACondition()
        {
            //Debug.Log("Ctor [" + _currentTypeStr + "]");
            //if (!string.IsNullOrEmpty(_currentTypeStr))
            //{
            //    Debug.Log("str is not null or empty");
            //    _currentType = Type.GetType(_currentTypeStr);
            //}

            #region Drawing Actions
            _drawingActions.Add(typeof(Int64).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
            {
                var mid = rect.width / 2f;
                //Debug.Log("int");
                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
                if (_selectedIdx != 0)
                    InputInt = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputInt);
                return 15;
            }));
            _drawingActions.Add(typeof(int).ToString(), new DrawingAction((Rect rect, out int selectedIndex) => _drawingActions[typeof(Int64).ToString()].Invoke(rect, out selectedIndex)));
            _drawingActions.Add(typeof(float).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
            {
                var mid = rect.width / 2f;
                //Debug.Log("float => " + mid);
                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
                if (_selectedIdx != 0)
                    InputFloat = EditorGUI.FloatField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputFloat);
                return 15;
            }));
            _drawingActions.Add(typeof(double).ToString(), new DrawingAction((Rect rect, out int selectedIndex) => _drawingActions[typeof(float).ToString()].Invoke(rect, out selectedIndex)));
            _drawingActions.Add(typeof(string).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
            {
                var mid = rect.width / 2f;
                //Debug.Log("string");
                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsString);
                if (_selectedIdx != 0)
                    InputString = EditorGUI.TextField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputString);
                return 15;
            }));
            #endregion

            #region Evaluate Actions
            _evaluateActions.Add(typeof(Int64).ToString(), new Func<object, bool>((obj) =>
            {
                var ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
                Debug.Log("Evaluate int : " + ConditionNumber + " Input=" + InputInt + " Value=" + obj);
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
            _evaluateActions.Add(typeof(int).ToString(), new Func<object, bool> ((obj) => _evaluateActions[typeof(Int64).ToString()].Invoke(obj)));
            _evaluateActions.Add(typeof(string).ToString(), new Func<object, bool>((obj) =>
            {
                var ConditionString = (CONDITION_STRING)_selectedIdx;
                Debug.Log("Evaluate string : " + ConditionString + " Input=" + InputString + " Value=" + obj);
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
                Debug.Log("Evaluate float : " + ConditionNumber + " Input=" + InputFloat + " Value=" + obj);
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
        /// Adds a drawing action to the list.
        /// </summary>
        public void AddDrawAction(string type, DrawingAction action)
        {
            _drawingActions.Add(type, action);
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

            if (!_drawingActions.ContainsKey(enumName))
            {
                _drawingActions.Add(enumName, new DrawingAction((Rect rect, out int selectedIndex) =>
                {
                    var t = Type.GetType(enumType);
                    var mid = rect.width / 2f;
                //Debug.Log("enum");
                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsString);
                    if (string.IsNullOrEmpty(InputEnum))
                    {
                        Debug.Log("Activator is receiving type " + t.ToString());
                        InputEnum = Activator.CreateInstance(t).ToString();
                    }
                    if (_selectedIdx != 0)
                        InputEnum = EditorGUI.EnumPopup(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), (Enum)Enum.Parse(t, InputEnum)).ToString();
                    return 15;
                }));
            }

            if (!_evaluateActions.ContainsKey(enumName))
            {
                _evaluateActions.Add(enumName, new Func<object, bool>((obj) =>
                {
                    var ConditionEnum = (CONDITION_STRING)_selectedIdx;
                    Debug.Log("Evaluate enum : " + ConditionEnum + " Input=" + InputEnum + " Value=" + obj);
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
            Debug.Log("Set current type to => " + type);
            _currentTypeStr = type;
            //_currentType = Type.GetType(_currentTypeStr);
            _selectedIdx = 0;
        }

        public void SetRefOutput(ConditionInput<int> output)
        {
            Debug.Log("Setting ref output : " + output);
            refOutputInt = output;
        }

        public void SetRefOutput(ConditionInput<string> output)
        {
            refOutputString = output;
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
            Debug.Log("2. Evaluate with type => " + _evaluateActions[_currentTypeStr]);
            return _evaluateActions[_currentTypeStr].Invoke(val);
        }

        /// <summary>
        /// Retrives a class instance corresponding to the input type given.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ACondition GetConditionFromType(Type type)
        {
            return Activator.CreateInstance(_matchingTypes[type]) as ACondition;
        }

        /// <summary>
        /// Evaluates a set of conditions, returning true if all of them are satisfied.
        /// </summary>
        /// <param name="cdts"></param>
        /// <returns></returns>
        public static bool EvaluateSet(IEnumerable<ACondition> cdts)
        {
            throw new NotImplementedException("Evaluate set is not supported.");
            //foreach(var cdt in cdts)
            //{
            //    if (!cdt.Evaluate())
            //        return false;
            //}
            //return true;
        }

        /// <summary>
        /// Draws the condition and return its size.
        /// </summary>
        /// <returns></returns>
        public virtual float Draw(UnityEngine.Rect rect)
        {
            Debug.Log("1. Drawing => " + _currentTypeStr);
            //if (_currentType == null && !string.IsNullOrEmpty(_currentTypeStr))
            //    _currentType = Type.GetType(_currentTypeStr);
            //Debug.Log("2. Drawing => " + Type.GetType(_currentTypeStr));
            //if (_currentType != null)
            if (!string.IsNullOrEmpty(_currentTypeStr))
                return _drawingActions[_currentTypeStr].Invoke(rect, out _selectedIdx);
            return 0;
        }

        public void SetRefOutput<T>(ConditionInput<T> cdt)
        {
            _currentEvaluator.SetRefOutput(cdt);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            //Debug.Log("On after desserialize !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + _registeredTypes.Count);
            foreach (var item in _registeredTypes)
            {
                RegisterEnum(item);
            }
        }
    }
}