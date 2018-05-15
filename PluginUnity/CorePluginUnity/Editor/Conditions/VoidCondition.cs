namespace Core.Plugin.Unity.Editor.Conditions
{
    /// <summary>
    /// Represents a condition that is always satisfied.
    /// </summary>
    public class VoidCondition : ACondition
    {
        public override bool Evaluate()
        {
            return true;
        }
    }
}