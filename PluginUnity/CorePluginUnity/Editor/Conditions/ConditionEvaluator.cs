using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Conditions
{
    /// <summary>
    /// Represents a condition for an integer input.
    /// </summary>
    [System.Serializable]
    public class ConditionEvaluator
    {
        public enum CONDITION { NO_CONDITION, MORE, LESS, EQUAL, DIFFERENT }

        public int Input;
        public CONDITION Condition;

        [SerializeField]
        private ConditionInput<int> refOutput;
        private string[] _options = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };

        [SerializeField]
        private int _selectedIdx;

        public void SetRefOutput(ConditionInput<int> output)
        {
            refOutput = output;
        }

        public bool Evaluate()
        {
            switch (Condition)
            {
                case CONDITION.NO_CONDITION:
                    return true;

                case CONDITION.MORE:
                    return refOutput.Value > Input;

                case CONDITION.LESS:
                    return refOutput.Value < Input;

                case CONDITION.EQUAL:
                    return refOutput.Value == Input;

                case CONDITION.DIFFERENT:
                    return refOutput.Value != Input;
            }
            return false;
        }

        public float Draw(Rect rect)
        {
            Debug.Log("mid => ");

            var mid = rect.width / 2f;
            Debug.Log("mid => " + mid);
            _selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, _options);
            if (_selectedIdx != 0)
                Input = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), Input);
            Condition = (CONDITION)_selectedIdx;
            return 15;
        }

        public void SetRefOutput<T>(ConditionInput<T> output)
        {
            refOutput = output as ConditionInput<int>;
        }
    }
}