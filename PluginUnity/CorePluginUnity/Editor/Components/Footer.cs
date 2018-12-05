using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components
{
    class Footer : IDrawable
    {

        private readonly GUIStyle _link; 
        public Footer()
        {
            _link = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(0.416f, 0.686f, 0.745f) },
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };
        }

        public void Draw()
        {

            GUILayout.BeginArea(new Rect(0, Screen.height - 60, Screen.width, 60));
            GUILayout.BeginVertical();
            if (GUILayout.Button("Online tutorial", _link))
            {
                Application.OpenURL("https://dnai.io/tutorial/plugin/");
            }
            if (GUILayout.Button("Join us at dnai.io", _link))
            {
                Application.OpenURL("https://dnai.io/");
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
