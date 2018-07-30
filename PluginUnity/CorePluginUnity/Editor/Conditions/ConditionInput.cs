namespace Core.Plugin.Unity.Editor.Conditions
{
    /// <summary>
    /// Represents an input wrapper for a condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public class ConditionInput<T>
    {
        public T Value;

        /// <summary>
        /// Makes the use of the wrapper easier.
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator ConditionInput<T>(T input)
        {
            //UnityEngine.Debug.Log("Implicit operator Condition Input with value => " + input);
            return new ConditionInput<T>() { Value = input };
        }

        /// <summary>
        /// Makes the use of the wrapper easier.
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator T(ConditionInput<T> input)
        {
            return input.Value;
        }
    }
}