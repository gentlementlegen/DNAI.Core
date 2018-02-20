using System;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Handles the drawing of the settings widow for the DNAI editor.
    /// </summary>
    public class SettingsDrawer : EditorWindow
    {
        private Settings _settings;

        public SettingsDrawer()
        {
            titleContent = new GUIContent("DNAI Settings");
        }

        public void OnEnable()
        {
            //Debug.Log("On enable called");
            hideFlags = HideFlags.HideAndDontSave;

            if (_settings == null)
            {
                //Debug.Log("On enable TEST IS NULL");
                _settings = new Settings();
            }
            //else
            //{
            //    Debug.Log("On enable TEST is NOT null");
            //}
        }

        private void OnGUI()
        {
            GUILayout.Label("Hello" + (_settings.MyInt++));
        }
    }

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public int MyInt;
    }
}