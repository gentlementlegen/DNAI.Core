using Core.Plugin.Unity.Context;
using Core.Plugin.Unity.Editor;
using Core.Plugin.Unity.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Plugin.Unity.Drawing
{
    /// <summary>
    /// This class helps Unity remembering the state of the window content between each close/open call.
    /// </summary>
    [System.Serializable]
    internal class EditorSettings : ScriptableObject
    {
        [SerializeField]
        public List<ScriptDrawer.ListAIHandler> listIA = new List<ScriptDrawer.ListAIHandler>();
    }

    /// <summary>
    /// Draws the <code>ScriptManager</code> class to the window.
    /// </summary>
    public class ScriptDrawer : /*EditorWindow, ScriptableObject,*/ IEditorDrawable
    {
        public static GUIContent iconToolbarPlus;
        public static GUIContent dotButton;
        public static GUIContent refreshButton;

        public static GUIStyle preButton;
        public static GUIStyle miniButton;

        private ReorderableList rList;

        private List<ListAIHandler> listIA;

        private EditorWindow _editorWindow;
        private EditorSettings _editorSettings;

        private const string rootPath = "Assets/Standard Assets/DNAI/";

        /// Should the ScriptDrawer be drawing ?
        public bool ShouldDraw
        { get; set; }

        /// <summary>
        /// Nested class for the IA list.
        /// Contains a ScriptManager and a Reordarable list to draw.
        /// </summary>
        [System.Serializable]
        internal class ListAIHandler
        {
            [SerializeField]
            public ScriptManager scriptManager;

            /// <summary>
            /// Backup of the <see cref="SubScriptList"/> list that cannot be serialized by Unity.
            /// </summary>
            [SerializeField]
            private List<string> _scriptList = new List<string>();

            private ReorderableList _subScriptList;

            /// <summary>
            /// List of scripts that can be displayed in the Unity UI.
            /// </summary>
            public ReorderableList SubScriptList
            {
                get
                {
                    if (_subScriptList == null && scriptManager != null && scriptManager.FunctionList != null && scriptManager.FunctionList != null)
                    {
                        _subScriptList = new ReorderableList(scriptManager.FunctionList, scriptManager.FunctionList.GetType(), false, true, false, false)
                        {
                            drawHeaderCallback = DrawHeaderInternal,
                            drawElementCallback = DrawListElement
                        };
                    }
                    return _subScriptList;
                }
            }

            /// <summary>
            /// Gets the current size of the script list.
            /// </summary>
            public float CurrentSize
            {
                get
                {
                    return 80f + (SubScriptList?.count * 20f) ?? 80f;
                }
            }

            private List<bool> _selectedScripts;

            /// <summary>
            /// Enables this class in order to be used.
            /// Necessary because Unity cannot serialize classes that do request to any UI element
            /// inside a ctor, as the ReordarableList does.
            /// </summary>
            public void OnEnable()
            {
                if (scriptManager == null)
                    scriptManager = new ScriptManager();
                ////subScriptList = new ReorderableList(scriptManager.iaList, scriptManager.iaList.GetType(), false, true, false, false);

                //subScriptList = new ReorderableList(scriptManager.FunctionList, scriptManager.FunctionList.GetType(), false, true, false, false);
                //subScriptList.drawHeaderCallback = DrawHeaderInternal;
                //subScriptList.drawElementCallback = DrawListElement;

                //Debug.Log("[ScriptDrawer IA HANDLER] ================== OnEnable ia list => " + _scriptList.Count);
                if (scriptManager.FunctionList?.Count <= 0)
                {
                    foreach (var func in _scriptList)
                        scriptManager.FunctionList.Add(func);
                }
            }

            /// <summary>
            /// Must be called manually when the UI is disabled, to preserve the workspace.
            /// </summary>
            public void OnDisable()
            {
                //Debug.Log("[ScriptDrawer IA HANDLER] ======================= ia list => " + _scriptList.Count);
                if (scriptManager?.FunctionList != null)
                {
                    _scriptList.Clear();
                    //Debug.Log("[ScriptDrawer IAHANDLER] ======================= FUNCTION ia list => " + scriptManager.FunctionList.Count);
                    foreach (var func in scriptManager.FunctionList)
                        _scriptList.Add(func);
                }
            }

            /// <summary>
            /// Draws the header of the list.
            /// </summary>
            /// <param name="rect">Rect size</param>
            private void DrawHeaderInternal(Rect rect)
            {
                Rect refreshRect = new Rect(rect.x + rect.xMax - 25f, rect.y, 15f, 15f);

                EditorGUI.LabelField(rect, "IA List");

                //subScriptList.list = scriptManager.iaList;
                SubScriptList.list = scriptManager.FunctionList;
                if (_selectedScripts?.Count != SubScriptList.list.Count)
                    _selectedScripts = Enumerable.Repeat(false, SubScriptList.list.Count).ToList();

                if (GUI.Button(refreshRect, refreshButton))
                {
                    UnityTask.Run(async () =>
                    {
                        //await scriptManager.CompileAsync(_selectedScripts.FindIndices(x => x));
                        await scriptManager.CompileAsync();
                        AssetDatabase.ImportAsset("Assets/Standard Assets/DNAI/Compiled/" + scriptManager.AssemblyName + ".dll");
                    }).ContinueWith((e) =>
                    {
                        if (e.IsFaulted)
                        {
                            Debug.LogError(e?.Exception.GetBaseException().Message);
                            Debug.LogError(e?.Exception.GetBaseException().StackTrace);
                        }
                    });
                }
            }

            /// <summary>
            /// Draws an element in the list.
            /// </summary>
            /// <param name="rect">Rect.</param>
            /// <param name="index">Index.</param>
            /// <param name="isAtive">If set to <c>true</c> is active.</param>
            /// <param name="isFocused">If set to <c>true</c> is focused.</param>
            private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                Rect labelRect = new Rect(rect.x, rect.y, rect.xMax - 45f, 15f);
                Rect plusRect = new Rect(rect.xMax - 8f - 25f, rect.y, 25f, 13f);

                //string str = scriptManager.iaList[index].Key;
                string str = scriptManager.FunctionList[index];
                GUI.Label(labelRect, str);

                //_selectedScripts[index] = GUI.Toggle(plusRect, _selectedScripts[index], "");
           
                //if (GUI.Button(plusRect, iconToolbarPlus, preButton))
                //{
                //foreach (var go in Selection.gameObjects)
                //{
                //    go.AddComponent(scriptManager.iaList[index].Value);
                //}
                //}
                if (index + 1 < SubScriptList.count)
                    DrawingHelper.Separator(new Rect(labelRect.x, labelRect.y + EditorGUIUtility.singleLineHeight + 1.5f, rect.width, 1.2f));
            }
        }

        public ScriptDrawer()
        {
            CloudFileWatcher.FileCreated += (sender, e) =>
            {
                var newElem = new ListAIHandler();
                //newElem.scriptManager.FilePath = e.FullPath;
                //newElem.scriptManager.LoadScript();
                AddElementInternal(newElem);// todo fill path and stuff
            };
        }

        /// <summary>
        /// Called when the window is enabled.
        /// </summary>
        public void OnEnable()
        {
            ShouldDraw = true;
            iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "|Add new script");
            dotButton = EditorGUIUtility.IconContent("sv_icon_dot0_sml", "|Browse Script");
            refreshButton = EditorGUIUtility.IconContent("TreeEditor.Refresh", "|Refresh");
            LoadSettings();
            listIA = _editorSettings.listIA;
            //Debug.Log(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ON ENABLE CALLED");
            listIA.ForEach(x => x.OnEnable());
            rList = new ReorderableList(listIA, typeof(ScriptManager));
            rList.draggable = false;
            rList.elementHeight *= 2;
            rList.drawHeaderCallback = DrawHeaderInternal;
            rList.drawElementCallback = DrawElementInternal;
            rList.onAddCallback = AddElementInternal;
            //_editorWindow = EditorWindow.GetWindow(typeof(DulyEditor));
            _editorWindow = DulyEditor.Instance;
        }

        /// <summary>
        /// Called when the window is disabled.
        /// </summary>
        public void OnDisable()
        {
            //Debug.Log("+++++++ List count => " + _editorSettings.listIA.Count);
            listIA.ForEach(x => x.OnDisable());
        }

        public void OnDestroy()
        {
            //Debug.Log("+++++++ ON DESTROY List count => " + _editorSettings.listIA.Count);
            listIA.ForEach(x => x.OnDisable());
        }

        /// <summary>
        /// Retrieves the saved setting of the wokspace.
        /// </summary>
        private void LoadSettings()
        {
            Directory.CreateDirectory(rootPath);
            _editorSettings = (EditorSettings)AssetDatabase.LoadAssetAtPath<EditorSettings>(rootPath + "DNAIEditor.asset");
            if (_editorSettings == null)
            {
                //Debug.Log("Settings were NULL");
                //_editorSettings = CreateInstance<EditorSettings>();
                _editorSettings = ScriptableObject.CreateInstance<EditorSettings>();
                AssetDatabase.CreateAsset(_editorSettings, rootPath + "DNAIEditor.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset(rootPath + "DNAIEditor.asset");
            }
            //Debug.Log("[Settings] list => " + _editorSettings.listIA.Count);
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(_editorSettings);
        }

        /// <summary>
        /// Draws the scripts, if <paramref name="ShouldDraw"/> is True.
        /// </summary>
        public void Draw()
        {
            if (ShouldDraw)
            {
                rList.DoLayoutList();
            }
        }

        /// <summary>
        /// Draws the header if the loaded script list.
        /// </summary>
        /// <param name="rect">Rect.</param>
        private void DrawHeaderInternal(Rect rect)
        {
            EditorGUI.LabelField(rect, "Loaded Scripts");
        }

        /// <summary>
        /// Draws an element in the loaded script list.
        /// </summary>
        /// <param name="rect">Rect.</param>
        /// <param name="index">Index.</param>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        /// <param name="isFocused">If set to <c>true</c> is focused.</param>
        private void DrawElementInternal(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.yMin += 3f;
            rect.yMax -= 7f;

            var thirdWidth = rect.width * 0.85f;
            var halfHeight = rect.height / 2;

            //			var gameObjectRect = new Rect( rect.x, rect.y, thirdWidth, halfHeight );
            //			var dropdownRect = new Rect( rect.x + thirdWidth, rect.y, thirdWidth * 2, halfHeight );
            //			var bottomRect = new Rect( rect.x, rect.y + halfHeight, rect.width, halfHeight );
            var gameObjectRect = new Rect(rect.x, rect.y, thirdWidth, 15f);
            var dropdownRect = new Rect(rect.x + thirdWidth, rect.y, rect.width * 0.15f, 15f);
            var bottomRect = new Rect(rect.x, rect.y + 50f, rect.width, 15f);

            //Debug.Log("[DEBUG] list ia => " + listIA);
            //Debug.Log("[DEBUG] list ia => " + listIA[index]);
            //Debug.Log("[DEBUG] list ia => " + listIA[index].scriptManager);
            //Debug.Log("[DEBUG] list ia => " + listIA[index].scriptManager.FilePath);

            listIA[index].scriptManager.FilePath = GUI.TextField(gameObjectRect, listIA[index].scriptManager.FilePath);

            if (miniButton == null)
                miniButton = "miniButton";
            if (preButton == null)
                preButton = "RL FooterButton";

            //Debug.Log("[DEBUG] 1. ");

            //			if (GUI.Button (dropdownRect, "Browse", miniButton))
            if (GUI.Button(dropdownRect, dotButton))
            {
                var newPath = EditorUtility.OpenFilePanel("Select a script to load", "Documents", Constants.iaFileExtension);
                if (!string.IsNullOrEmpty(newPath))
                {
                    listIA[index].scriptManager.FilePath = newPath;
                    listIA[index].scriptManager.LoadScript();
                    AssetDatabase.Refresh();
                    _editorWindow.Focus();
                }
            }

            //Debug.Log("[DEBUG] 2. ");

            //GUI.Label(bottomRect, listIA[index].scriptManager.ProcessingStatus);

            //Debug.Log("[DEBUG] 3.");

            listIA[index].SubScriptList?.DoList(new Rect(rect.x, rect.y + 30, rect.width, rect.height));

            if (listIA[index]?.CurrentSize > rList.elementHeight)
            {
                rList.elementHeight = listIA[index].CurrentSize;
            }

            //Debug.Log("[DEBUG] 4. ");

            _editorWindow.Repaint();
        }

        private void AddElementInternal(ReorderableList list)
        {
            var handler = new ListAIHandler();
            handler.OnEnable();
            listIA.Add(handler);
        }

        private void AddElementInternal(ListAIHandler handler)
        {
            handler.OnEnable();
            listIA.Add(handler);
        }
    }
}