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
    }
}