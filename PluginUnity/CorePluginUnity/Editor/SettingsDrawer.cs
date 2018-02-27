using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Context;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Handles the drawing of the settings widow for the DNAI editor.
    /// </summary>
    public class SettingsDrawer : EditorWindow
    {
        public const string RootPath = "Assets/Standard Assets/DNAI/";
        public const string FileName = "DNAIEditorSettings.asset";

        private Settings _settings;

        private string _connectionStatus = "Disconnected.";

        public SettingsDrawer()
        {
            titleContent = new GUIContent("DNAI Settings");
        }

        public void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
            LoadSettings();
        }

        public void OnDestroy()
        {
        }

        private void LoadSettings()
        {
            Directory.CreateDirectory(RootPath);
            _settings = AssetDatabase.LoadAssetAtPath<Settings>(RootPath + FileName);
            if (_settings == null)
            {
                _settings = ScriptableObject.CreateInstance<Settings>();
                AssetDatabase.CreateAsset(_settings, RootPath + FileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(RootPath + FileName);
            }
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(_settings);
        }

        private void OnGUI()
        {
            GUILayout.Label("Credentials");
            GUILayout.Label("Username");
            _settings.Username = GUILayout.TextField(_settings.Username);
            GUILayout.Label("Password");
            _settings.Password = GUILayout.PasswordField(_settings.Password, '*', 25);
            GUILayout.Label(_connectionStatus);
            if (GUILayout.Button("Login"))
            {
                UnityTask.Run(async() =>
                {
                    _connectionStatus = "Connecting...";
                    var token = await CloudFileWatcher.Access.GetToken(_settings.Username, _settings.Password);
                    if (token.access_token != null)
                    {
                        _connectionStatus = "Connected.";
                        CloudFileWatcher.Access.SetAuthorization(token);
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
                CloudFileWatcher.Access.DownloadSolution();
            }
            _settings.AutoSync = GUILayout.Toggle(_settings.AutoSync, "Automatically sync files");
            CloudFileWatcher.Watch(_settings.AutoSync);
        }
    }

    [Serializable]
    public class Settings : ScriptableObject
    {
        public string Username = "";
        public string Password = "";
        public bool AutoSync = true;
    }
}