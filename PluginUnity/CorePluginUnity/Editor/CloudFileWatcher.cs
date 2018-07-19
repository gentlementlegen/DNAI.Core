using Core.Plugin.Unity.API;
using Core.Plugin.Unity.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Handles the watching of the script folder, to synchronize actions with file updates.
    /// </summary>
    public static class CloudFileWatcher
    {
        /// <summary>
        /// Called on file creation.
        /// </summary>
        public static event FileSystemEventHandler FileCreated { add => _fileWatcher.Created += value; remove => _fileWatcher.Created -= value; }

        /// <summary>
        /// Called on file changed.
        /// </summary>
        public static event FileSystemEventHandler FileChanged { add => _fileWatcher.Changed += value; remove => _fileWatcher.Changed -= value; }

        private static readonly FileSystemWatcher _fileWatcher = new FileSystemWatcher();

        internal static readonly ApiAccess Access = new ApiAccess();

        private static readonly Queue<Func<Task<HttpResponseMessage>>> _delayedActions = new Queue<Func<Task<HttpResponseMessage>>>();

        static CloudFileWatcher()
        {
            Directory.CreateDirectory(Constants.ScriptPath);
            _fileWatcher.Path = (Constants.ScriptPath);
            _fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            _fileWatcher.Filter = "*." + Constants.iaFileExtension;
            //_fileWatcher.Created += OnFileCreated;
            //_fileWatcher.Changed += OnFileChanged;
            //_fileWatcher.Deleted += OnFileDeleted;
            _fileWatcher.EnableRaisingEvents = true;
            //StartWatcher();
        }

        private static void StartWatcher()
        {
            UnityTask.Run(async () =>
            {
                while (true)
                {
                    if (_fileWatcher.EnableRaisingEvents && _delayedActions.Count > 0)
                    {
                        var q = _delayedActions.Dequeue();
                        Debug.Log("Dequeuing task");
                        await UnityTask.Run(async () =>
                        {
                            var ret = await q.Invoke();
                            if (!ret.IsSuccessStatusCode)
                                throw new HttpRequestException(ret.ReasonPhrase);
                        }).ContinueWith((arg) =>
                        {
                            Debug.Log("Continue with status => " + arg.Status);
                            if (arg.Status != TaskStatus.RanToCompletion)
                                _delayedActions.Enqueue(q);
                        });
                        Debug.Log("End dequeued task");
                    }
                await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// Downloads a file asynchronously to the script folder. Returns true on succes, false otherwise.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        internal static async Task<bool> DownloadFileAsync(string userID, API.File file)
        {
            var fileContent = await Access.GetFileContent(userID, file._id);
            if (fileContent == null)
                return false;
            var stream = System.IO.File.Create(Constants.ScriptPath + file.Title + "." + Constants.iaFileExtension);
            stream.Write(fileContent, 0, fileContent.Length);
            stream.Dispose();
            AssetDatabase.ImportAsset(Constants.ScriptPath + file.Title + "." + Constants.iaFileExtension);
            return true;
        }

        /// <summary>
        /// Sets the watcher on pause or active.
        /// </summary>
        /// <param name="watch"></param>
        public static void Watch(bool watch)
        {
            _fileWatcher.EnableRaisingEvents = watch;
        }

        private static void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File changed " + e.Name);

            var func = new Func<Task<HttpResponseMessage>>(async () =>
            {
                var ret = await Access.PutFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response put = " + ret);
                return ret;
            });
        }

        private static void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File deleted " + e.Name);
            var func = new Func<Task<HttpResponseMessage>>(async () =>
            {
                var ret = await Access.DeleteFile(Path.GetFileName(e.Name));
                Debug.Log("Response delete = " + ret);
                return ret;
            });

            _delayedActions.Enqueue(func);
        }

        private static void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            Debug.Log("[Settings drawer] File created " + e.Name);

            var func = new Func<Task<HttpResponseMessage>>(async () =>
            {
                var ret = await Access.PostFile(new FileUpload { file_type_id = 1, in_store = false, title = Path.GetFileName(e.Name), file = e.Name });
                Debug.Log("Response Create = " + ret);
                return ret;
            });

            _delayedActions.Enqueue(func);
        }
    }
}