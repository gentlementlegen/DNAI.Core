using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            var type = controller.GetEntityType(entity.Id);
            if (type == ENTITY.ENUM_TYPE)
            {
                var ret = "";
                ret += $"enum {entity.Name} {{";
                foreach (var v in controller.GetEnumerationValues(entity.Id))
                    ret += $"{v} = {controller.GetEnumerationValue(entity.Id, v)},";
                ret += "}";
                return ret;
            }
            //var t = controller.GetVariableType(entity.Id);
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

        /// <summary>
        /// Makes the first letter of a string Uppercase.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UppercaseFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Removes illegal characters in a string such as space, dashes.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveIllegalCharacters(this string str)
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            str = rgx.Replace(str, "").UppercaseFirst();
            str = r.Replace(str, "");
            var invalidChars = new string(Path.GetInvalidFileNameChars()) + " -";
            return string.Concat(str.Split(invalidChars.ToCharArray()));
            //return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rgx.Replace(str, ""));
        }

        /// <summary>
        /// Converts a string to its encoded base 64 version.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Finds the indices of all objects matching the given predicate.
        /// </summary>
        /// <typeparam name="T">The type of objects in the list.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Indices of all objects matching the given predicate.</returns>
        public static IEnumerable<int> FindIndices<T>(this IList<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).Select(list.IndexOf);
        }
    }
}