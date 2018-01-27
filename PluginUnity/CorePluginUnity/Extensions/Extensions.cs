using static CoreControl.EntityFactory;

namespace Core.Plugin.Unity.Extensions
{
    /// <summary>
    /// Class extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Converts an Entity to a serializable string, aka
        /// a fully qualified type as you would see it in source code.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="controller">The controller where the entity is stored.</param>
        /// <returns>The serializable string.</returns>
        public static string ToSerialString(this Entity entity, CoreControl.Controller controller)
        {
            var value = controller.GetVariableValue(entity.Id);
            return $"{value.GetType()} {entity.Name}";
        }

        /// <summary>
        /// Splits a string according to its camel case represetation, adding spaces in it.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SplitCamelCase(this string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}