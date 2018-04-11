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
        public const string FileName = "DNAIEditorSettings.asset";

        public static string UserID { get; private set; }

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
            _connectionStatus = CloudFileWatcher.Access.Token != null ? "Connected." : "Disconnected.";
        }

        public void OnDestroy()
        {
        }

        private void LoadSettings()
        {
            Directory.CreateDirectory(Constants.RootPath);
            _settings = AssetDatabase.LoadAssetAtPath<Settings>(Constants.RootPath + FileName);
            if (_settings == null)
            {
                _settings = ScriptableObject.CreateInstance<Settings>();
                AssetDatabase.CreateAsset(_settings, Constants.RootPath + FileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(Constants.RootPath + FileName);
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
            var password = "";
            password = GUILayout.PasswordField(password, '*', 25);
            GUILayout.Label(_connectionStatus);
            if (GUILayout.Button("Login"))
            {
                UnityTask.Run(async () =>
                {
                    _connectionStatus = "Connecting...";
                    Token token = null;
                    try
                    {
                        token = await CloudFileWatcher.Access.GetToken(_settings.Username, password);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.InnerException.Message);
                    }
                    if (token.token != null)
                    {
                        _connectionStatus = "Connected.";
                        CloudFileWatcher.Access.SetAuthorization(token);
                        UserID = token.user_id;
                    }
                    else
                    {
                        _connectionStatus = "Wrong user/password.";
                    }
                    Repaint();
                });
            }
            if (GUILayout.Button("Download"))
            {
                CloudFileWatcher.Access.DownloadSolution();
            }
            _settings.AutoLogin = GUILayout.Toggle(_settings.AutoLogin, "Remember me");
            //CloudFileWatcher.Watch(_settings.AutoLogin);
        }
    }

    [Serializable]
    public class Settings : ScriptableObject
    {
        [HideInInspector]
        public string Username = "";
        [HideInInspector]
        public string Token = "";
        [HideInInspector]
        public bool AutoLogin = true;
    }
}