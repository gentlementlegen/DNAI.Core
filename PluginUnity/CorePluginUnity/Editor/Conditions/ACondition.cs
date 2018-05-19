using System;
using System.Collections.Generic;
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

        public CallbackFunc Callback;

        public string TestACondition;

        [SerializeField]
        private string _currentTypeStr = "System.Int64";

        [SerializeField]
        private Type _currentType = typeof(int);

        private ConditionEvaluator _currentEvaluator;

        public ACondition()
        {
            Debug.Log("Ctor " + _currentTypeStr);
            _currentType = Type.GetType(_currentTypeStr);
        }

        /// <summary>
        /// Evaluates if the condition is satisfied.
        /// </summary>
        /// <returns></returns>
        public virtual bool Evaluate()
        {
            return _matchingInstanceTypes[0].Evaluate();
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
            UnityEngine.Debug.Log("drawing");
            if (_currentType != null)
                _matchingInstanceTypes[0].Draw(rect);
            return 0;
        }

        public void SetRefOutput<T>(ConditionInput<T> cdt)
        {
            _currentEvaluator.SetRefOutput(cdt);
        }
    }
}