using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components
{
    class ToolsBar : IDrawable
    {
        private readonly List<Button> _buttons = new List<Button>();
        private readonly int _spacing;

        public ToolsBar(int spacing = 0)
        {
            _spacing = spacing;
        }

        public void AppendButton(Button button)
        {
            if (button == null) return;
            _buttons.Add(button);
        }

        public void Draw()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            foreach (var button in _buttons)
            {
                button.Draw();
                GUILayout.Space(_spacing);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }


}
