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
    public class SettingsDrawer : EditorWindow, IDisposable
    {
        private Settings _settings;
        private static readonly ApiAccess _access = new ApiAccess();
        private static readonly FileSystemWatcher _fileWatcher = new FileSystemWatcher();

        private string _username = "";
        private string _password = "";
        private bool _autoSync = true;
        private string _connectionStatus = "Disconnected.";

        public SettingsDrawer()
        {
            titleContent = new GUIContent("DNAI Settings");
            _fileWatcher.Path = ("Assets/Standard Assets/DulyAssets/Scripts/");
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Filter = "*.duly";
            _fileWatcher.Created += OnFileCreated;
            _fileWatcher.Changed += OnFileChanged;
            _fileWatcher.Deleted += OnFileDeleted;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File changed " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.PutFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response put = " + ret);
            }).ContinueWith((x) => Debug.Log("File put ? " + x.Status));
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File deleted " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.DeleteFile(Path.GetFileName(e.Name));
                Debug.Log("Response delete = " + ret);
            }).ContinueWith((x) => Debug.Log("File deleted ? " + x.Status));
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File created " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.PostFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response upload = " + ret);
            }).ContinueWith((x) => Debug.Log("File uploaded ? " + x.Status));
        }

        public void OnDestroy()
        {
            Dispose();
        }

        public void OnDisable()
        {
            Dispose();
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
            GUILayout.Label("Credentials");
            GUILayout.Label("Username");
            _username = GUILayout.TextField(_username);
            GUILayout.Label("Password");
            _password = GUILayout.PasswordField(_password, '*', 25);
            GUILayout.Label(_connectionStatus);
            if (GUILayout.Button("Login"))
            {
                UnityTask.Run(async() =>
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
            _autoSync = GUILayout.Toggle(_autoSync, "Automatically sync files");
            _fileWatcher.EnableRaisingEvents = _autoSync;
        }

        public void Dispose()
        {
            _fileWatcher.Created -= OnFileCreated;
            _fileWatcher.Changed -= OnFileChanged;
            _fileWatcher.Deleted -= OnFileDeleted;
        }
    }

    [Serializable]
    public class Settings
    {
        [SerializeField]
        public int MyInt;
    }
}