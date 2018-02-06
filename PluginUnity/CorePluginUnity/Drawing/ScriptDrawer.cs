using Core.Plugin.Editor;
using Core.Plugin.Unity.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Plugin.Drawing
{
    /// <summary>
    /// This class helps Unity remembering the state of the window content between each close/open call.
    /// </summary>
    internal class EditorSettings : ScriptableObject
    {
        public readonly List<ScriptDrawer.ListAIHandler> listIA = new List<ScriptDrawer.ListAIHandler>();
    }

    /// <summary>
    /// Draws the <code>ScriptManager</code> class to the window.
    /// </summary>
    public class ScriptDrawer : EditorWindow, IEditorDrawable
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

        /// Should the ScriptDrawer be drawing ?
        public bool ShouldDraw
        { get; set; }

        /// <summary>
        /// Nested class for the IA list.
        /// Contains a ScriptManager and a Reordarable list to draw.
        /// </summary>
        internal class ListAIHandler
        {
            public ScriptManager scriptManager;
            public ReorderableList subScriptList;

            public float CurrentSize
            {
                get
                {
                    return 80f + (subScriptList.count * 20f);
                }
            }

            private List<bool> _selectedScripts;

            public ListAIHandler()
            {
                scriptManager = new ScriptManager();
                //subScriptList = new ReorderableList(scriptManager.iaList, scriptManager.iaList.GetType(), false, true, false, false);
                subScriptList = new ReorderableList(scriptManager.FunctionList, scriptManager.FunctionList.GetType(), false, true, false, false);
                subScriptList.drawHeaderCallback = DrawHeaderInternal;
                subScriptList.drawElementCallback = DrawListElement;
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
                subScriptList.list = scriptManager.FunctionList;
                if (_selectedScripts?.Count != subScriptList.list.Count)
                    _selectedScripts = Enumerable.Repeat(false, subScriptList.list.Count).ToList();

                if (GUI.Button(refreshRect, refreshButton))
                {
                    scriptManager.Compile(_selectedScripts.FindIndices(x => x));
                    //AssetDatabase.Refresh();
                    AssetDatabase.ImportAsset("Assets/Plugins/" + scriptManager.AssemblyName + ".dll");
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

                _selectedScripts[index] = GUI.Toggle(plusRect, _selectedScripts[index], "");
                //if (GUI.Button(plusRect, iconToolbarPlus, preButton))
                //{
                    //foreach (var go in Selection.gameObjects)
                    //{
                    //    go.AddComponent(scriptManager.iaList[index].Value);
                    //}
                //}
                if (index + 1 < subScriptList.count)
                    DrawingHelper.Separator(new Rect(labelRect.x, labelRect.y + EditorGUIUtility.singleLineHeight + 1.5f, rect.width, 1.2f));
            }
        }

        /// <summary>
        /// Called when the window is enabled.
        /// </summary>
        private void OnEnable()
        {
            ShouldDraw = true;
            iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "|Add new script");
            dotButton = EditorGUIUtility.IconContent("sv_icon_dot0_sml", "|Browse Script");
            refreshButton = EditorGUIUtility.IconContent("TreeEditor.Refresh", "|Refresh");
            LoadSettings();
            listIA = _editorSettings.listIA;
            rList = new ReorderableList(listIA, typeof(ScriptManager));
            rList.draggable = false;
            rList.elementHeight *= 2;
            rList.drawHeaderCallback = DrawHeaderInternal;
            rList.drawElementCallback = DrawElementInternal;
            rList.onAddCallback = AddElementInternal;
            _editorWindow = EditorWindow.GetWindow(typeof(DulyEditor));
        }

        private void LoadSettings()
        {
            _editorSettings = (EditorSettings)AssetDatabase.LoadAssetAtPath<EditorSettings>("Assets/DulyAssets/DulyEditor.asset");
            if (_editorSettings == null)
            {
                _editorSettings = CreateInstance<EditorSettings>();
                AssetDatabase.CreateAsset(_editorSettings, "Assets/DulyAssets/DulyEditor.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.ImportAsset("Assets/DulyAssets/DulyEditor.asset");
            }
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

            listIA[index].scriptManager.FilePath = GUI.TextField(gameObjectRect, listIA[index].scriptManager.FilePath);

            if (miniButton == null)
                miniButton = "miniButton";
            if (preButton == null)
                preButton = "RL FooterButton";

            //			if (GUI.Button (dropdownRect, "Browse", miniButton))
            if (GUI.Button(dropdownRect, dotButton))
            {
                listIA[index].scriptManager.FilePath = EditorUtility.OpenFilePanel("Select a script to load", "Documents", Constants.iaFileExtension);
                listIA[index].scriptManager.LoadScript();
                AssetDatabase.Refresh();
                _editorWindow.Focus();
            }

            GUI.Label(bottomRect, listIA[index].scriptManager.ProcessingStatus);

            listIA[index].subScriptList.DoList(new Rect(rect.x, rect.y + 30, rect.width, rect.height));
            if (listIA[index].CurrentSize > rList.elementHeight)
            {
                rList.elementHeight = listIA[index].CurrentSize;
            }
            _editorWindow.Repaint();
        }

        private void AddElementInternal(ReorderableList list)
        {
            listIA.Add(new ListAIHandler());
        }
    }
}