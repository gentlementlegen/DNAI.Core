namespace Core.Plugin.Unity.Editor.Conditions
{
    /// <summary>
    /// Represents a condition for an integer input.
    /// </summary>
    [System.Serializable]
    public class IntCondition : ACondition
    {
        public enum CONDITION { MORE, LESS, EQUAL, DIFFERENT }

        private ConditionInput<int> refOutput;
        public int Input;
        public CONDITION Condition;

        public void SetRefOutput(ConditionInput<int> output)
        {
            refOutput = output;
        }

        public override bool Evaluate()
        {
            switch (Condition)
            {
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
    }
}