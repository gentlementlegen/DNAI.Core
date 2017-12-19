using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Drawing
{
    /// <summary>
    /// Drawing helper, that contains generic drawing tools for Unity Windows.
    /// </summary>
    public static class DrawingHelper
    {
        /// Separator styles
        private static readonly Color s_SeparatorColor;

        private static readonly GUIStyle s_SeparatorStyle;

        /// <summary>
        /// Initializes the <see cref="CorePlugin.Drawing.DrawingHelper"/> class.
        /// </summary>
        static DrawingHelper()
        {
            s_SeparatorColor = new Color(0.5f, 0.5f, 0.5f);

            s_SeparatorStyle = new GUIStyle();
            s_SeparatorStyle.normal.background = EditorGUIUtility.whiteTexture;
            s_SeparatorStyle.stretchWidth = true;
        }

        /// <summary>
        /// Draws a separator of a certain size in a certain color.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="color">Color.</param>
        public static void Separator(Rect position, Color color)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Color restoreColor = GUI.color;
                GUI.color = color;
                s_SeparatorStyle.Draw(position, false, false, false, false);
                GUI.color = restoreColor;
            }
        }

        /// <summary>
        /// Draws a separator at a certain position.
        /// </summary>
        /// <param name="position">Position.</param>
        public static void Separator(Rect position)
        {
            Separator(position, s_SeparatorColor);
        }
    }
}