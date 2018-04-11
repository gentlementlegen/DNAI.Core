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
        private string _password = "";

        public SettingsDrawer()
        {
            titleContent = new GUIContent("DNAI Settings");
        }

        public void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;
            LoadSettings();
            if (_settings.AutoLogin)
                CloudFileWatcher.Access.SetAuthorization(_settings.Token);
            _connectionStatus = !CloudFileWatcher.Access.Token.IsEmpty() ? "Connected." : "Disconnected.";
        }

        /// <summary>
        /// Called when the window is destroyed.
        /// </summary>
        public void OnDestroy()
        {
            if (!_settings.AutoLogin)
            {
                _settings.Username = "";
                _settings.Token = null;
            }
        }

        /// <summary>
        /// Allows loading the settings stored from previous session.
        /// </summary>
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

        /// <summary>
        /// Called when drawing the GUI.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Credentials");
            GUILayout.Label("Username");
            _settings.Username = GUILayout.TextField(_settings.Username);
            GUILayout.Label("Password");
            _password = GUILayout.PasswordField(_password, '*', 25);
            GUILayout.Label(_connectionStatus);
            if (GUILayout.Button("Login"))
            {
                UnityTask.Run(async () =>
                {
                    _connectionStatus = "Connecting...";
                    Token token = null;
                    try
                    {
                        token = await CloudFileWatcher.Access.GetToken(_settings.Username, _password);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.InnerException.Message);
                    }
                    if (token.token != null)
                    {
                        SetToken(token);
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

        /// <summary>
        /// Sets a token for the current session.
        /// </summary>
        /// <param name="token"></param>
        private void SetToken(Token token)
        {
            _connectionStatus = "Connected.";
            CloudFileWatcher.Access.SetAuthorization(token);
            UserID = token.user_id;
            _settings.Token = token;
        }
    }

    /// <summary>
    /// Class that helps saving the window settings.
    /// </summary>
    [Serializable]
    public class Settings : ScriptableObject
    {
        [HideInInspector]
        public string Username = "";
        //[HideInInspector]
        public Token Token;
        [HideInInspector]
        public bool AutoLogin = true;
    }
}