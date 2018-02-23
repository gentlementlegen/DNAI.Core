using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Context;
using System.IO;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    public static class CloudFileWatcher
    {
        private static readonly FileSystemWatcher _fileWatcher = new FileSystemWatcher();
        private static readonly ApiAccess _access = new ApiAccess();

        static CloudFileWatcher()
        {
            _fileWatcher.Path = ("Assets/Standard Assets/DNAI/Scripts/");
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Filter = "*.duly";
            _fileWatcher.Created += OnFileCreated;
            _fileWatcher.Changed += OnFileChanged;
            _fileWatcher.Deleted += OnFileDeleted;
            _fileWatcher.EnableRaisingEvents = true;
        }

        public static void Watch(bool watch)
        {
            Debug.Log("Status => " + _fileWatcher.EnableRaisingEvents);
            _fileWatcher.EnableRaisingEvents = watch;
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File changed " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.PutFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response put = " + ret);
            }).ContinueWith((x) => Debug.Log("File put ? " + x.Status));
        }

        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File deleted " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.DeleteFile(Path.GetFileName(e.Name));
                Debug.Log("Response delete = " + ret);
            }).ContinueWith((x) => Debug.Log("File deleted ? " + x.Status));
        }

        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File created " + e.Name);
            UnityTask.Run(async () =>
            {
                var ret = await _access.PostFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response upload = " + ret);
            }).ContinueWith((x) => Debug.Log("File uploaded ? " + x.Status));
        }
    }
}