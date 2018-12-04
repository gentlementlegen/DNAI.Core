using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Plugin.Unity.Editor.Components
{
    class Button : IDrawable
    {
        protected readonly Texture texture;
        protected readonly string name;
        protected readonly GUIStyle style;
        private readonly GUIContent _ct;

        public delegate void ClickAction();

        public event ClickAction OnClicked;

        protected readonly Color _backgroundColor;
        protected readonly Color _contentColor;

        public Button(string name, Texture texture)
        {
            this.texture = texture;
            this.name = name;
            style = new GUIStyle(GUI.skin.button) {border = new RectOffset(2, 2, 2, 2)};
            style.normal.textColor = _contentColor;
            //style.normal.
            _ct = new GUIContent(texture, name);
            _contentColor = DulyEditor.FontColor;
            _backgroundColor = DulyEditor.BackgroundColor;
        }

        public virtual void Draw()
        {
            var bc = GUI.backgroundColor;
            var cc = GUI.contentColor;
            GUI.backgroundColor = _backgroundColor;
            GUI.contentColor = _contentColor;

            if (GUILayout.Button(_ct, style, GUILayout.Width(50), GUILayout.Height(50)))
            {
                OnClicked?.Invoke();
            }

            GUI.backgroundColor = bc;
            GUI.contentColor = cc;
        }
    }
}
