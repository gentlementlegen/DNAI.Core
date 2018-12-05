using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components.Buttons
{
    class MLButton : ToolBarButton
    {
        private float progress = 0;
        private long bytesReceived;
        private long bytesToreceive;
        private int percentage;
        private static bool mlStatusInit = false;
        private static string dependenciesPath;
        private bool shouldCloseProgress = false;
        private bool shouldCleanDependencies = false;
        private WebClient wc;
        private string downloadStatus;

        public static DulyEditor.ML_STATUS _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;

        private readonly Color[] _mlStatusColor = {
            new Color(1, 0.28f, 0.28f), //RED
            new Color(1f, 0.76f, 0.28f), //ORANGE
            new Color(0.24f, 0.69f, 0.42f), //GREEN
            new Color(1, 1f, 0.28f) //YELLOW
        };

        public MLButton() : base("Ml Package", AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "machine_learning.png"))
        {

        }

        public override void Draw()
        {
            var old = GUI.contentColor;

            if (wc == null)
            {
                wc = new WebClient();
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                dependenciesPath = Application.dataPath;
                mlStatusInit = false;
                Task.Run(() => ValidateDependencies());
            }
            var ct = new GUIContent(texture, "Machine Learning - " + (_mlStatus == DulyEditor.ML_STATUS.INSTALLED ? "Enabled" : "Disabled"));
            GUI.contentColor = _mlStatusColor[(int)_mlStatus];
            if (GUILayout.Button(ct, GUILayout.Width(50), GUILayout.Height(50)) && mlStatusInit)
            {
                switch (_mlStatus)
                {
                    case DulyEditor.ML_STATUS.NOT_INSTALLED:
                        try
                        {
                            wc.DownloadFileAsync(new Uri(Constants.MlUrl), Application.dataPath + "/../Dnai.ML.PluginDependencies.zip");
                            _mlStatus = DulyEditor.ML_STATUS.DOWNLOADING;
                        }
                        catch (Exception e)
                        {
                            _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;
                            shouldCloseProgress = true;
                            Debug.Log(e.Message);
                        }
                        break;
                    case DulyEditor.ML_STATUS.DOWNLOADING:
                        break;
                    case DulyEditor.ML_STATUS.INSTALLED:
                        shouldCleanDependencies = true;
                        break;
                    case DulyEditor.ML_STATUS.UNINSTALLING:
                        break;
                    default:
                        break;
                }
            }

            GUI.contentColor = old;

            if (_mlStatus == DulyEditor.ML_STATUS.DOWNLOADING ||
                _mlStatus == DulyEditor.ML_STATUS.UNINSTALLING)
            {
                DulyEditor.Instance.Repaint();
            }

            if (shouldCloseProgress)
            {
                shouldCloseProgress = false;
                EditorUtility.ClearProgressBar();
            }
            else if (shouldCleanDependencies)
            {
                shouldCleanDependencies = false;
                Task.Run(() => CleanDependencies());
            }
            else
            {
                switch (_mlStatus)
                {
                    case DulyEditor.ML_STATUS.NOT_INSTALLED:
                        break;
                    case DulyEditor.ML_STATUS.DOWNLOADING:
                        if (EditorUtility.DisplayCancelableProgressBar("Downloading Machine Learning Package",
                            downloadStatus, progress))
                        {
                            downloadStatus = "Cancelling...";
                            wc?.CancelAsync();
                        }
                        else
                        {
                            downloadStatus = $"Downloading content {bytesReceived}/{bytesToreceive}MB ({percentage}%)";
                        }
                        break;
                    case DulyEditor.ML_STATUS.INSTALLED:
                        break;
                    case DulyEditor.ML_STATUS.UNINSTALLING:
                        EditorUtility.DisplayProgressBar("Uninstalling Machine Learning Package",
                            $"Uninstalling content {bytesReceived}/{bytesToreceive} Files ({percentage}%)", progress);
                        break;
                    default:
                        break;
                }
            }
        }


        private void Wc_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var archivePath = Application.dataPath + "/../Dnai.ML.PluginDependencies.zip";
            if (e.Cancelled)
            {
                wc.Dispose();
                wc = null;
                shouldCleanDependencies = true;
                return;
            }
            try
            {
                ZipFile.ExtractToDirectory(archivePath, Application.dataPath + "/../");
                System.IO.File.Delete(archivePath);
            }
            catch (IOException ioe)
            {
                Debug.LogWarning("On Machine Learning download: " + ioe.Message);
            }
            shouldCloseProgress = true;
            ValidateDependencies();
            if (_mlStatus == DulyEditor.ML_STATUS.NOT_INSTALLED)
                shouldCleanDependencies = true;
        }

        private void CleanDependencies()
        {
            _mlStatus = DulyEditor.ML_STATUS.UNINSTALLING;
            var archivePath = dependenciesPath + "/../Dnai.ML.PluginDependencies.zip";
            if (System.IO.File.Exists(archivePath))
                System.IO.File.Delete(archivePath);
            var dependencies = System.IO.File.ReadAllLines(Constants.PluginsPath + "/dependencies.txt");
            var depPath = dependenciesPath + "/../";
            int count = 0;
            bytesToreceive = dependencies.Count();
            foreach (var depName in dependencies)
            {
                bytesReceived = count;
                progress = (float)count++ / dependencies.Count();
                percentage = (int)(progress * 100f);
                var md5 = depPath + "checksum_" + depName + ".md5";
                var dll = depPath + depName + ".dll";
                if (System.IO.File.Exists(dll))
                    System.IO.File.Delete(dll);
                if (System.IO.File.Exists(md5))
                    System.IO.File.Delete(md5);
            }
            bytesReceived = count;
            progress = 1f;
            percentage = 100;
            _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;
            shouldCloseProgress = true;
        }

        public static string[] GetDependencyList()
        {
            return File.ReadAllLines(Constants.PluginsPath + "/dependencies.txt");
        }

        public static bool ValidateDependenciesStatus()
        {
            ValidateDependencies();
            return _mlStatus == DulyEditor.ML_STATUS.INSTALLED;
        }

        private static void ValidateDependencies()
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    var dependencies = GetDependencyList();
                    var depPath = dependenciesPath + "/../";
                    foreach (var depName in dependencies)
                    {
                        var dll = depPath + depName + ".dll";
                        var md5Name = depPath + "checksum_" + depName + ".md5";
                        if (!System.IO.File.Exists(dll) || !System.IO.File.Exists(md5Name))
                        {
                            _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;
                            mlStatusInit = true;
                            return;
                        }

                        using (var streamDll = System.IO.File.OpenRead(dll))
                        {
                            var bytes = System.IO.File.ReadAllBytes(md5Name);
                            var dllBytes = md5.ComputeHash(streamDll);
                            if (bytes.SequenceEqual(dllBytes)) continue;
                            _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;
                            mlStatusInit = true;
                            return;
                        }

                    }
                }
                _mlStatus = DulyEditor.ML_STATUS.INSTALLED;
                mlStatusInit = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                _mlStatus = DulyEditor.ML_STATUS.NOT_INSTALLED;
                mlStatusInit = true;
            }
        }

        private void Wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progress = e.ProgressPercentage / 100f;
            bytesReceived = ConvertBytesToMegabytes(e.BytesReceived);
            bytesToreceive = ConvertBytesToMegabytes(e.TotalBytesToReceive);
            percentage = e.ProgressPercentage;
        }

        static long ConvertBytesToMegabytes(long bytes)
        {
            return (long)((bytes / 1024f) / 1024f);
        }

    }
}
