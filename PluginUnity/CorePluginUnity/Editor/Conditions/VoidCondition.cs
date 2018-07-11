namespace Core.Plugin.Unity.Editor.Conditions
{
    /// <summary>
    /// Represents a condition that is always satisfied.
    /// </summary>
    public class VoidCondition : ACondition
    {
        public override float Draw(UnityEngine.Rect rect)
        {
            throw new System.NotImplementedException();
        }
    }
}