using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components
{
    class Header : IDrawable
    {
        private readonly Texture _logoTexture;
        private readonly Vector2 _bounds = new Vector2(150f, 400f);
        private readonly float _ratio;
        private GUIStyle _style;
        private readonly int _fontSize;
        public Header(Texture texture)
        {
            _logoTexture = texture;
            _ratio = (float)_logoTexture.width / _logoTexture.height;
            _fontSize = 24;
            _style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter, fontSize = _fontSize, normal = {textColor = Color.white}
            };
        }

        public void Draw()
        {
            if (_logoTexture == null) return;

            GUILayout.BeginVertical();

            float height = Mathf.Clamp(Screen.width / _ratio, _bounds.x, _bounds.y);
            float width = Mathf.Clamp(Screen.width, _ratio * _bounds.x, _ratio * _bounds.y);
            float percent = height / _bounds.y;
            _style.fontSize = (int)(_fontSize * percent);
            GUI.DrawTexture(new Rect((Screen.width - width) / 2, 10, width, height), _logoTexture);
            GUI.Label(new Rect(0, height * .76f, Screen.width, 30f), "Design Node for Artificial Intelligence", _style);
            GUILayout.Space(height - 20);

            GUILayout.EndVertical();

        }
    }
}
