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
using Core.Plugin.Unity.Editor.Components;
using Core.Plugin.Unity.Editor.Components.Buttons;
using Debug = UnityEngine.Debug;

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
        public ScriptDrawer ScriptDrawer { get; set; }
        public static Color FontColor = new Color(0.95f, 0.95f, 0.95f);
        public static Color BackgroundColor = new Color(0.259f, 0.259f, 0.259f);

        private OnlineScriptDrawer _onlineScriptDrawer;
        public SettingsDrawer SettingsDrawer { get; set; }
        private static DulyEditor _window;
        private static Texture _texture;
        private static GUIContent _settingsContent;
        //[SerializeField]
        //private bool _isMlDownloading;

        private Vector2 scrollPos;
        public bool IsCompiling { get; set; }

        public static DulyEditor Instance { get { return _window; } }
        public static GUISkin Skin;


        private Background _background;
        private Header _header;
        private Footer _footer;
        private ToolsBar _toolsBar;

        public enum ML_STATUS
        {
            NOT_INSTALLED = 0,
            DOWNLOADING,
            INSTALLED,
            UNINSTALLING
        };

        public DulyEditor()
        {
            _window = this;
            _window.minSize = new Vector2(300f, 300f);
            _window.maxSize = new Vector2(800f, 800f);
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
            Debug.Log("Show");
            if (_window == null)
            {
                _window = (DulyEditor)EditorWindow.GetWindow(typeof(DulyEditor));
                _texture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "dnai_logo.png");
                _window.titleContent = new GUIContent("DNAI", _texture);
                _window.Show();
            }
            else
            {
                //_window.Close();
                _window.Focus();
            }
        }

        private void OnGUI()
        {

            GUI.enabled = !EditorApplication.isPlaying;
            if (Skin == null)
                Skin = AssetDatabase.LoadAssetAtPath<GUISkin>(Constants.ResourcesPath + "DNAI_EditorSkin.guiskin");
            var old = GUI.skin;
            GUI.skin = DulyEditor.Skin;

            if (_settingsContent == null)
                _settingsContent = EditorGUIUtility.IconContent("SettingsIcon", "|Settings");

            if (SettingsDrawer == null)
            {
                SettingsDrawer = CreateInstance<SettingsDrawer>();
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            DrawPage();

            EditorGUILayout.Space();

            DrawToolsBar();

            EditorGUILayout.Space();

            DrawContent();

            _footer.Draw();

            GUILayout.EndScrollView();
            GUI.skin = old;
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;

            //Debug.Log("[DulyEditor] On enable");
            if (ScriptDrawer == null)
            {
                //ScriptDrawer = ScriptableObject.CreateInstance<ScriptDrawer>();
                ScriptDrawer = new ScriptDrawer();
                ScriptDrawer.OnEnable();
                //_isMlDownloading = ScriptDrawer.EditorSettings.IsMlEnabled;
            }
            if (SettingsDrawer == null)
            {
                SettingsDrawer = CreateInstance<SettingsDrawer>();
            }
            if (_onlineScriptDrawer == null)
            {
                _onlineScriptDrawer = new OnlineScriptDrawer();
                SettingsDrawer.OnConnection += (t, e) =>
                {
                    if (e.IsSuccess)
                        _onlineScriptDrawer.FetchFiles();
                };
            }
        }

        private void OnDisable()
        {
            //Debug.Log("[DulyEditor] On disable");
            ScriptDrawer?.OnDisable();
            AssetDatabase.SaveAssets();
        }

        private void OnDestroy()
        {
            ScriptDrawer?.OnDestroy();
            AssetDatabase.SaveAssets();
        }

        #region Editor Drawing
        private void DrawPage()
        {
            GUILayout.BeginHorizontal();

            if (_background == null)
            {
                _background = new Background(new Color(0.208f, 0.2f, 0.29f));
                _header = new Header(AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "logo_color.png"));
                _footer = new Footer();
            }
            _background.Draw();
            _header.Draw();

            GUILayout.EndHorizontal();
        }
        
        private void DrawContent()
        {
            GUI.enabled = !IsCompiling;

            GUILayout.MaxWidth(400f);

            ScriptDrawer?.Draw();
            EditorGUILayout.Space();
            _onlineScriptDrawer?.Draw();
            EditorGUI.EndDisabledGroup();
            
            if (MLButton.mlStatusInit && MLButton._mlStatus == ML_STATUS.NOT_INSTALLED )
                EditorGUILayout.HelpBox(
                    "DNAI Machine learning package isn't install.\nClic on the brain icon if you need Machine Learning in your project.",
                    MessageType.Info);
        }

        /// <summary>
        /// Draws the build button to the window.
        /// </summary>
        private void DrawToolsBar()
        {
            if (_toolsBar == null)
            {
                _toolsBar = new ToolsBar();
                _toolsBar.AppendButton(new BuildButton());
                _toolsBar.AppendButton(new SettingsButton());
                _toolsBar.AppendButton(new MLButton());
            }
            _toolsBar.Draw();
        }

        #endregion Editor Drawing
    }
}