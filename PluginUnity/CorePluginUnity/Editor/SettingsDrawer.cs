using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Context;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    public class OnConnectionEvent : EventArgs
    {
        public bool IsSuccess;
    }

    /// <summary>
    /// Handles the drawing of the settings widow for the DNAI editor.
    /// </summary>
    public class SettingsDrawer : EditorWindow
    {
        public const string FileName = "DNAIEditorSettings.asset";

        public static string UserID { get; private set; }

        public event EventHandler<OnConnectionEvent> OnConnection;

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
            {
                _connectionStatus = "";
                UnityTask.Run(() => TryLogin());
                //CloudFileWatcher.Access.SetAuthorization(_settings.Token);
            }
            //_connectionStatus = !CloudFileWatcher.Access.Token.IsEmpty() ? "Connected." : "Disconnected.";
        }

        /// <summary>
        /// Tests a connexion to the server with the previous used token.
        /// Since it's a not permanent connexion, we just try accessing files.
        /// </summary>
        private async void TryLogin()
        {
            //_connectionStatus = "";
            //Token token = null;
            System.Collections.Generic.List<API.File> list = null;
            try
            {
                //token = await CloudFileWatcher.Access.GetToken(_settings.Username, _password);
                list = await CloudFileWatcher.Access.GetFiles(_settings.Token.user_id);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.InnerException.Message);
            }
            //Debug.Log("Tried to login with id => " + _settings.Username + " token " + _settings.Token.user_id + " list is " + list);
            if (list != null)
            {
                //CloudFileWatcher.Access.SetAuthorization(_settings.Token);
                //_connectionStatus = "Connected.";
                SetConnected(_settings.Token);
            }
            else
            {
                //_connectionStatus = "Disconnected.";
                SetDisconnected();
            }
            Repaint();
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
            OnConnection = null;
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
                        SetConnected(token);
                    }
                    else
                    {
                        //_connectionStatus = "Wrong user/password.";
                        SetDisconnected("Wrong user/password.");
                    }
                    Repaint();
                });
            }
            if (GUILayout.Button("Logout"))
            {
                SetDisconnected();
            }
            _settings.AutoLogin = GUILayout.Toggle(_settings.AutoLogin, "Remember me");
            if (GUILayout.Button("Download"))
            {
                CloudFileWatcher.Access.DownloadSolution();
            }
            //CloudFileWatcher.Watch(_settings.AutoLogin);
        }

        /// <summary>
        /// Sets a token for the current session.
        /// </summary>
        /// <param name="token"></param>
        private void SetConnected(Token token)
        {
            _connectionStatus = "Connected.";
            CloudFileWatcher.Access.SetAuthorization(token);
            UserID = token.user_id;
            _settings.Token = token;
            OnConnection?.Invoke(this, new OnConnectionEvent { IsSuccess = true });
        }

        private void SetDisconnected(string message = "Disconnected.")
        {
            _connectionStatus = message;
            CloudFileWatcher.Access.SetAuthorization(null);
            UserID = "";
            _settings.Token = null;
            OnConnection?.Invoke(this, new OnConnectionEvent { IsSuccess = false });
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
        [HideInInspector]
        public Token Token;
        [HideInInspector]
        public bool AutoLogin = true;
    }
}