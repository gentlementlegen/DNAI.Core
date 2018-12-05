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
        private readonly List<ToolBarButton> _buttons = new List<ToolBarButton>();
        private readonly int _spacing;

        public ToolsBar(int spacing = 0)
        {
            _spacing = spacing;
        }

        public void AppendButton(ToolBarButton button)
        {
            if (button == null) return;
            _buttons.Add(button);
        }

        public ToolBarButton GetButton(int index)
        {
            return _buttons[index];
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
