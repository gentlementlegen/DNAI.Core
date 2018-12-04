using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components
{
    public class Background : IDrawable
    {
        private readonly Color _backgroundColor;

        public Background(Color color)
        {
            _backgroundColor = color;
        }

        public void Draw()
        {
            Color old = GUI.backgroundColor;
            GUI.backgroundColor = _backgroundColor;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.backgroundColor = old;
        }
    }
}