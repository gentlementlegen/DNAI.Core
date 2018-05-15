using System;
using System.Collections.Generic;

namespace Core.Plugin.Unity.Editor.Conditions
{
    public delegate void CallbackFunc();

    /// <summary>
    /// Abstract class for conditions related to Unity GUI callbacks.
    /// </summary>
    [Serializable]
    public class ACondition
    {
        private static readonly Dictionary<Type, Type> _matchingTypes = new Dictionary<Type, Type>
        {
            { typeof(int), typeof(IntCondition)  },
            { typeof(void), typeof(VoidCondition) }
        };

        public CallbackFunc Callback;

        public string TestACondition;

        /// <summary>
        /// Evaluates if the condition is satisfied.
        /// </summary>
        /// <returns></returns>
        public virtual bool Evaluate() { return false; }

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
    }
}