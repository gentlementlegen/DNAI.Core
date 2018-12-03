using System;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Core.Plugin.Unity.Drawing;
using UnityEditor;
using UnityEngine;
using File = Core.Plugin.Unity.API.File;
using System.IO;
using System.Linq;

/// <summary>
/// Duly editor.
/// </summary>

// See also : https://msdn.microsoft.com/en-us/library/dd997372.aspx
// Custom editor example : https://github.com/Thundernerd/Unity3D-ExtendedEvent
// Unity event code source : https://bitbucket.org/Unity-Technologies/ui/src/0155c39e05ca5d7dcc97d9974256ef83bc122586/UnityEditor.UI/EventSystem/EventTriggerEditor.cs?at=5.2&fileviewer=file-view-default
// List of icons : https://gist.github.com/MattRix/c1f7840ae2419d8eb2ec0695448d4321
// Advanced reorderable list : https://forum.unity3d.com/threads/reorderable-list-v2.339717/
// Atlassian to advanced reordarable list : https://bitbucket.org/rotorz/reorderable-list-editor-field-for-unity
// Order list in editor : https://forum.unity3d.com/threads/reorderablelist-in-the-custom-editorwindow.384006/
// Unity Decompiled : https://github.com/MattRix/UnityDecompiled/blob/master/UnityEditor/UnityEditorInternal/ReorderableList.cs
// Saving window state : https://answers.unity.com/questions/119978/how-do-i-have-an-editorwindow-save-its-data-inbetw.html
// Serialization rules in Unity : https://blogs.unity3d.com/2012/10/25/unity-serialization/
// Good doc on reorderable lists : http://va.lent.in/unity-make-your-lists-functional-with-reorderablelist/

// TODO : L'idée ce serait de faire un système comme les unity events, ou on chargerait un script,
// qui une fois chargé (avec des threads) afficherait les behaviours dispos
// Pour ce qui est de l'affichage, ce serait comme les unity events, mais avec un zone
// de texte pour mettre le path à la place du truc pour selectionner les objets, et au bout un bouton load.
// Si le fichier est bien load, ça montre tous les behaviours disponibles en dessous.

// Note : pour unity, si la .dll référence d'autres .dll cela provoque un crash au runtime. Deux solutions :
// soit copier toutes les .dll dans le dossier plugin, soit utiliser un utilitaire de merge de librairies
// cf : https://github.com/Microsoft/ILMerge/blob/master/ilmerge-manual.md

namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Duly editor, the unique, the one.
    /// </summary>
    public class DulyEditor : EditorWindow
    {
        private ScriptDrawer _scriptDrawer;
        private OnlineScriptDrawer _onlineScriptDrawer;
        private SettingsDrawer _settingsDrawer;
        private static DulyEditor _window;
        private static Texture _texture;
        private static Texture _buildTexture;
        private static Texture _settingsTexture;
        private static Texture _logoTexture;
        private static GUIContent _settingsContent;
        private Texture _mlTexture;
        [SerializeField]
        private bool _isMlDownloading;

        private Vector2 scrollPos;
        private bool _isCompiling;

        public static DulyEditor Instance { get { return _window; } }

        private WebClient wc;
        private string downloadStatus;

        public DulyEditor()
        {
            _window = this;
            //CloudFileWatcher.Watch(true);
        }

        //[MenuItem("Window/Duly")]
        //static void Init()
        //{
        //    // Get existing open window or if none, make a new one:
        //    if (_window == null)
        //    {
        //        _window = (DulyEditor)EditorWindow.GetWindow(typeof(DulyEditor));
        //        _window.titleContent = new GUIContent("Duly");
        //        _window.Show();
        //    }
        //    else
        //    {
        //        _window.Focus();
        //    }
        //    //DulyEditor window = (DulyEditor)EditorWindow.GetWindow(typeof(DulyEditor));
        //    //_window.titleContent = new GUIContent("Duly");
        //    //_window.Show();
        //}

        [MenuItem("Window/DNAI &d")]
        public static void ShowWindow()
        {
            if (_window == null)
            {
                _window = (DulyEditor)EditorWindow.GetWindow(typeof(DulyEditor));
                _texture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "dnai_logo.png");
                _window.titleContent = new GUIContent("DNAI", _texture);
                _window.Show();
            }
            else
            {
                _window.Focus();
            }
        }

        private void OnGUI()
        {
            GUI.enabled = !EditorApplication.isPlaying;

            if (_settingsContent == null)
                _settingsContent = EditorGUIUtility.IconContent("SettingsIcon", "|Settings");

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            DrawWindowTitle();

            if (_settingsDrawer == null)
                _settingsDrawer = CreateInstance<SettingsDrawer>();
            //if (GUILayout.Button(_settingsContent))
            //{
            //    //if (_settingsDrawer == null)
            //        //_settingsDrawer = CreateInstance<SettingsDrawer>();
            //    _settingsDrawer?.ShowAuxWindow();
            //}
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            DrawBuildButton();

            GUI.enabled = !_isCompiling;

            _scriptDrawer?.Draw();
            EditorGUILayout.Space();
            _onlineScriptDrawer?.Draw();
            EditorGUI.EndDisabledGroup();

            GUILayout.EndScrollView();
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;

            //Debug.Log("[DulyEditor] On enable");
            if (_scriptDrawer == null)
            {
                //_scriptDrawer = ScriptableObject.CreateInstance<ScriptDrawer>();
                _scriptDrawer = new ScriptDrawer();
                _scriptDrawer.OnEnable();
                _isMlDownloading = _scriptDrawer.EditorSettings.IsMlEnabled;
            }
            if (_settingsDrawer == null)
            {
                _settingsDrawer = CreateInstance<SettingsDrawer>();
            }
            if (_onlineScriptDrawer == null)
            {
                _onlineScriptDrawer = new OnlineScriptDrawer();
                _settingsDrawer.OnConnection += (t, e) =>
                {
                    if (e.IsSuccess)
                        _onlineScriptDrawer.FetchFiles();
                };
            }
        }

        private void OnDisable()
        {
            //Debug.Log("[DulyEditor] On disable");
            _scriptDrawer.EditorSettings.IsMlEnabled = _isMlDownloading;
            _scriptDrawer?.OnDisable();
            AssetDatabase.SaveAssets();
        }

        private void OnDestroy()
        {
            _scriptDrawer?.OnDestroy();
            AssetDatabase.SaveAssets();
        }

        #region Editor Drawing

        private void DrawWindowTitle()
        {
            GUILayout.FlexibleSpace();
            //GUILayout.Label("DNAI Editor", EditorStyles.largeLabel);
            if (_logoTexture == null)
                _logoTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "logo_color.png");

            GUILayout.Label(_logoTexture);
            GUILayout.FlexibleSpace();
        }

        private int _currentScriptCount;
        private int _maxScriptCount;

        /// <summary>
        /// Draws the build button to the window.
        /// </summary>
        private void DrawBuildButton()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (_buildTexture == null)
                _buildTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "build.png");
            GUIContent ct = new GUIContent(_buildTexture, "Build");

            // Build scripts button
            if (GUILayout.Button(ct, GUILayout.Width(50), GUILayout.Height(50)))
            {
                Context.UnityTask.Run(async () =>
                {
                    //await scriptManager.CompileAsync(_selectedScripts.FindIndices(x => x));
                    try
                    {
                        _isCompiling = true;
                        _maxScriptCount = _scriptDrawer.ListAI.Count;
                        for (int i = 0; i < _scriptDrawer.ListAI.Count; i++)
                        {
                            _currentScriptCount = i + 1;

                            if (EditorUtility.DisplayCancelableProgressBar("Compiling DNAI scripts",
                                    $"Processed {_currentScriptCount}/{_maxScriptCount} scripts", _currentScriptCount / (float)_maxScriptCount))
                            {
                                Debug.Log("Compilation canceled");
                                i = _scriptDrawer.ListAI.Count;
                                continue;
                            }

                            await _scriptDrawer.ListAI[i].scriptManager.CompileAsync();
                            AssetDatabase.ImportAsset(Constants.CompiledPath + _scriptDrawer.ListAI[i].scriptManager.AssemblyName + ".dll");
                        }
                        EditorUtility.ClearProgressBar();
                        _isCompiling = false;
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        Debug.LogError($"Could not find the DNAI file {ex.FileName}. Make sure it exists in the Scripts folder.");
                    }
                    finally
                    {
                        EditorUtility.ClearProgressBar();
                    }
                }).ContinueWith((e) =>
                {
                    if (e.IsFaulted)
                    {
                        Debug.LogError(e?.Exception.GetBaseException().Message + " " + e?.Exception.GetBaseException().StackTrace);
                    }
                    _isCompiling = false;
                });
            }

            // Settings button
            DrawSettingsButton();

            DrawMachineLearningButton();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            //if (_isCompiling)
            //{
            //    if (EditorUtility.DisplayCancelableProgressBar("Compiling DNAI scripts",
            //        $"Processed {_currentScriptCount}/{_maxScriptCount} scripts", _currentScriptCount / (float)_maxScriptCount))
            //    {
            //        Debug.Log("Compilation canceled");
            //    }
            //}
            //else
            //{
            //    EditorUtility.ClearProgressBar();
            //    //EditorGUIUtility.ExitGUI();
            //}
        }

        private void DrawSettingsButton()
        {
            // Settings button
            if (_settingsTexture == null)
                _settingsTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "settings.png");
            var ct = new GUIContent(_settingsTexture, "Settings");
            if (GUILayout.Button(ct, GUILayout.Width(50), GUILayout.Height(50)))
            {
                //if (_settingsDrawer == null)
                //_settingsDrawer = CreateInstance<SettingsDrawer>();
                _settingsDrawer?.ShowAuxWindow();
            }
        }


        private void DrawMachineLearningButton()
        {
            var old = GUI.contentColor;
            if (_mlTexture == null)
                _mlTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "machine_learning.png");

            if (wc == null)
            {
                wc = new WebClient();
                wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
                wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
                dependenciesPath = Application.dataPath;
                mlStatusInit = false;
                Task.Run(() => ValidateDependencies());
            }
            var ct = new GUIContent(_mlTexture, "Machine Learning - " + (_mlStatus == ML_STATUS.INSTALLED ? "Enabled" : "Disabled"));
            GUI.contentColor = _mlStatusColor[(int) _mlStatus];
            if (GUILayout.Button(ct, GUILayout.Width(50), GUILayout.Height(50)) && mlStatusInit)
            {
                switch (_mlStatus)
                {
                    case ML_STATUS.NOT_INSTALLED:
                        try
                        {
                            wc.DownloadFileAsync(new Uri(Constants.MlUrl), Application.dataPath + "/../Dnai.ML.PluginDependencies.zip");
                            _mlStatus = ML_STATUS.DOWNLOADING;
                        }
                        catch (Exception e)
                        {
                            _mlStatus = ML_STATUS.NOT_INSTALLED;
                            shouldCloseProgress = true;
                            Debug.Log(e.Message);
                        }
                        break;
                    case ML_STATUS.DOWNLOADING:
                        break;
                    case ML_STATUS.INSTALLED:
                        shouldCleanDependencies = true;
                        break;
                    case ML_STATUS.UNINSTALLING:
                        break;
                    default:
                        break;
                }
            }


            //Debug.Log(_mlStatus.ToString());
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
                    case ML_STATUS.NOT_INSTALLED:
                        break;
                    case ML_STATUS.DOWNLOADING:
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
                    case ML_STATUS.INSTALLED:
                        break;
                    case ML_STATUS.UNINSTALLING:
                        EditorUtility.DisplayProgressBar("Uninstalling Machine Learning Package",
                            $"Uninstalling content {bytesReceived}/{bytesToreceive} Files ({percentage}%)", progress);
                        break;
                    default:
                        break;
                }
            }

            GUI.contentColor = old;
        }

        private float progress = 0;
        private long bytesReceived;
        private long bytesToreceive;
        private int percentage;
        private bool mlStatusInit = false;
        private string dependenciesPath;
        private bool shouldCloseProgress = false;
        private bool shouldCleanDependencies = false;

        private enum ML_STATUS
        {
            NOT_INSTALLED = 0,
            DOWNLOADING,
            INSTALLED,
            UNINSTALLING
        };

        private ML_STATUS _mlStatus = ML_STATUS.NOT_INSTALLED;

        private readonly Color[] _mlStatusColor = {
            new Color(1, 0.28f, 0.28f), //RED
            new Color(1f, 0.76f, 0.28f), //ORANGE
            new Color(0.24f, 0.69f, 0.42f), //GREEN
            new Color(1, 1f, 0.28f) //YELLOW
        };

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
            if (_mlStatus == ML_STATUS.NOT_INSTALLED)
                shouldCleanDependencies = true;
        }

        private void CleanDependencies()
        {
            _mlStatus = ML_STATUS.UNINSTALLING;
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
            _mlStatus = ML_STATUS.NOT_INSTALLED;
            shouldCloseProgress = true;
        }

        private void ValidateDependencies()
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    var dependencies = System.IO.File.ReadAllLines(Constants.PluginsPath + "/dependencies.txt");
                    var depPath = dependenciesPath + "/../";
                    foreach (var depName in dependencies)
                    {
                        var dll = depPath + depName + ".dll";
                        var md5Name = depPath + "checksum_" + depName + ".md5";
                        if (!System.IO.File.Exists(dll) || !System.IO.File.Exists(md5Name))
                        {
                            _mlStatus = ML_STATUS.NOT_INSTALLED;
                            mlStatusInit = true;
                            return;
                        }

                        using (var streamDll = System.IO.File.OpenRead(dll))
                        {
                            var bytes = System.IO.File.ReadAllBytes(md5Name);
                            var dllBytes = md5.ComputeHash(streamDll);
                            if (bytes.SequenceEqual(dllBytes)) continue;
                            _mlStatus = ML_STATUS.NOT_INSTALLED;
                            mlStatusInit = true;
                            return;
                        }

                    }
                }
                _mlStatus = ML_STATUS.INSTALLED;
                mlStatusInit = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                _mlStatus = ML_STATUS.NOT_INSTALLED;
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

        #endregion Editor Drawing
    }
}