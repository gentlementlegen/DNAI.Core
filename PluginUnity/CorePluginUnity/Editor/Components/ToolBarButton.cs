using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Plugin.Unity.Editor.Components
{
    class ToolBarButton : IDrawable
    {
        protected readonly Texture texture;
        protected readonly string name;
        protected readonly GUIStyle style;
        private readonly GUIContent _ct;

        public delegate void ClickAction();

        public event ClickAction OnClicked;

        public ToolBarButton(string name, Texture texture)
        {
            this.texture = texture;
            this.name = name;
            style = new GUIStyle(GUI.skin.button) {border = new RectOffset(2, 2, 2, 2)};

            _ct = new GUIContent(texture, name);
        }

        public virtual void Draw()
        {
            if (GUILayout.Button(_ct, GUILayout.Width(50), GUILayout.Height(50)))
            {
                OnClicked?.Invoke();
            }
        }
    }
}
