using Core.Plugin.Unity.API;
using System;
using System.Threading.Tasks;
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
        private ApiAccess _access = new ApiAccess();

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
            GUILayout.Label("Credentials");
            GUILayout.Label("Username");
            GUILayout.TextField("toto");
            GUILayout.Label("Password");
            GUILayout.TextField("tata");
            if (GUILayout.Button("Login"))
            {
                Task.Run(() => _access.GetToken("toto", "tata"));
            }
            if (GUILayout.Button("Download"))
            {
                _access.DownloadSolution();
            }
        }
    }

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public int MyInt;
    }
}