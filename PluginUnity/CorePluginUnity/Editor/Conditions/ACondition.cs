using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Conditions
{
    public delegate void CallbackFunc();

    /// <summary>
    /// Abstract class for conditions related to Unity GUI callbacks.
    /// Unity does not support Serialization for derived classes,
    /// so this class handles every time that can be used within the editor.
    /// </summary>
    [Serializable]
    public class ACondition
    {
        private static readonly Dictionary<Type, Type> _matchingTypes = new Dictionary<Type, Type>
        {
            { typeof(int), typeof(ConditionEvaluator)  },
        };

        [SerializeField]
        private List<ConditionEvaluator> _matchingInstanceTypes = new List<ConditionEvaluator>
        {
            { new ConditionEvaluator()  },
        };

        [SerializeField]
        private int _selectedIdx;

        #region Number Options
        [SerializeField]
        public enum CONDITION_NUMBER { NO_CONDITION, MORE, LESS, EQUAL, DIFFERENT }
        public CONDITION_NUMBER ConditionNumber;
        private string[] optionsNumber = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };
        [SerializeField]
        private ConditionInput<int> refOutputInt;
        #endregion

        #region String Options
        [SerializeField]
        public enum CONDITION_STRING { NO_CONDITION, EQUAL, DIFFERENT }
        public CONDITION_STRING ConditionString;
        private string[] optionsString = new string[] { "No condition", "Equal to", "Different than" };
        [SerializeField]
        private ConditionInput<string> refOutputString;
        #endregion

        #region Input Serialized Values
        public int InputInt;
        public float InputFloat;
        public string InputString;
        #endregion

        /// <summary>
        /// List of drawing actions for given types.
        /// </summary>
        private readonly Dictionary<Type, Func<Rect, float>> _drawingActions = new Dictionary<Type, Func<Rect, float>>();
        private readonly Dictionary<Type, Func<bool>> _evaluateActions = new Dictionary<Type, Func<bool>>();

        public CallbackFunc Callback;

        [SerializeField]
        private string _currentTypeStr;// = "System.Int64";
        private Type _currentType;// = typeof(int);

        private ConditionEvaluator _currentEvaluator;

        public ACondition()
        {
            Debug.Log("Ctor [" + _currentTypeStr + "]");
            if (!string.IsNullOrEmpty(_currentTypeStr))
            {
                Debug.Log("str is not null or empty");
                _currentType = Type.GetType(_currentTypeStr);
            }

            #region Drawing Actions
            _drawingActions.Add(typeof(Int64), new Func<Rect, float>((Rect rect) =>
            {
                var mid = rect.width / 2f;
                Debug.Log("int");
                _selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
                if (_selectedIdx != 0)
                    InputInt = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputInt);
                ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
                return 15;
            }));
            _drawingActions.Add(typeof(int), new Func<Rect, float>((Rect rect) => _drawingActions[typeof(Int64)].Invoke(rect)));
            _drawingActions.Add(typeof(float), new Func<Rect, float>((Rect rect) =>
            {
                var mid = rect.width / 2f;
                Debug.Log("float => " + mid);
                _selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
                if (_selectedIdx != 0)
                    InputFloat = EditorGUI.FloatField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputFloat);
                ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
                return 15;
            }));
            _drawingActions.Add(typeof(double), new Func<Rect, float>((Rect rect) => _drawingActions[typeof(float)].Invoke(rect)));
            _drawingActions.Add(typeof(string), new Func<Rect, float>((Rect rect) =>
            {
                var mid = rect.width / 2f;
                Debug.Log("string");
                _selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsString);
                if (_selectedIdx != 0)
                    InputString = EditorGUI.TextField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputString);
                ConditionString = (CONDITION_STRING)_selectedIdx;
                return 15;
            }));
            #endregion

            #region Evaluate Actions
            _evaluateActions.Add(typeof(Int64), new Func<bool>(() =>
            {
                switch (ConditionNumber)
                {
                    case CONDITION_NUMBER.NO_CONDITION:
                        return true;

                    case CONDITION_NUMBER.MORE:
                        return refOutputInt.Value > InputInt;

                    case CONDITION_NUMBER.LESS:
                        return refOutputInt.Value < InputInt;

                    case CONDITION_NUMBER.EQUAL:
                        return refOutputInt.Value == InputInt;

                    case CONDITION_NUMBER.DIFFERENT:
                        return refOutputInt.Value != InputInt;
                }
                return false;
            }));
            _evaluateActions.Add(typeof(int), new Func<bool> (() => _evaluateActions[typeof(Int64)].Invoke()));
            #endregion
        }

        /// <summary>
        /// Sets the current type used, such as string, int...
        /// </summary>
        /// <param name="type"></param>
        public void SetCurrentType(string type)
        {
            Debug.Log("Set current type to => " + type);
            _currentTypeStr = type;
            _currentType = Type.GetType(_currentTypeStr);
            _selectedIdx = 0;
        }

        public void SetRefOutput(ConditionInput<int> output)
        {
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
        public virtual bool Evaluate()
        {
            Debug.Log("Evaluate with type => " + _currentType);
            return _evaluateActions[_currentType].Invoke();
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
            foreach(var cdt in cdts)
            {
                if (!cdt.Evaluate())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Draws the condition and return its size.
        /// </summary>
        /// <returns></returns>
        public virtual float Draw(UnityEngine.Rect rect)
        {
            Debug.Log("Drawing => " + _currentTypeStr);
            if (_currentType == null && _currentTypeStr != "")
                _currentType = Type.GetType(_currentTypeStr);
            if (_currentType != null)
                return _drawingActions[_currentType].Invoke(rect);
            return 0;
        }

        public void SetRefOutput<T>(ConditionInput<T> cdt)
        {
            _currentEvaluator.SetRefOutput(cdt);
        }
    }
}