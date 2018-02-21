using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Context;
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
        private readonly ApiAccess _access = new ApiAccess();

        private string _username;
        private string _password;

        private string _connectionStatus = "Disconnected.";

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
            _username = GUILayout.TextField(_username);
            GUILayout.Label("Password");
            _password = GUILayout.TextField(_password);
            GUILayout.Label(_connectionStatus);
            if (GUILayout.Button("Login"))
            {
                UnityTasks.Run(async() =>
                {
                    _connectionStatus = "Connecting...";
                    var token = await _access.GetToken(_username, _password);
                    if (token.access_token != null)
                    {
                        _connectionStatus = "Connected.";
                        _access.SetAuthorization(token);
                    }
                    else
                    {
                        _connectionStatus = "Error on getting access token.";
                    }
                    Repaint();
                });
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