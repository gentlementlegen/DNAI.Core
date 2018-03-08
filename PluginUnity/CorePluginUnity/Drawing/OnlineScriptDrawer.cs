using Core.Plugin.Unity.Context;
using Core.Plugin.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Core.Plugin.Unity.Drawing
{
    /// <summary>
    /// Drawer class for the DNAI script associated to the connected account.
    /// </summary>
    internal class OnlineScriptDrawer
    {
        private static GUIStyle preButton;

        private readonly ReorderableList _list;
        private readonly GUIContent iconToolbarPlus;
        private readonly List<string> _fileList = new List<string>();

        /// <summary>
        /// Gets the current size of the script list in pixels.
        /// </summary>
        public float CurrentSize
        {
            get
            {
                return 80f + (_list?.count * 20f) ?? 80f;
            }
        }

        internal OnlineScriptDrawer()
        {
            _list = new ReorderableList(_fileList, _fileList.GetType(), false, true, false, false)
            {
                drawHeaderCallback = DrawHeaderCallback,
                drawElementCallback = DrawElementCallback
            };
            iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "|Download script");
            preButton = "RL FooterButton";
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect labelRect = new Rect(rect.x, rect.y, rect.xMax - 45f, 15f);
            Rect plusRect = new Rect(rect.xMax - 8f - 25f, rect.y, 25f, 13f);

            string str = _fileList[index];
            GUI.Label(labelRect, str);

            if (GUI.Button(plusRect, iconToolbarPlus, preButton))
            {
            }

            if (index + 1 < _list.count)
                DrawingHelper.Separator(new Rect(labelRect.x, labelRect.y + EditorGUIUtility.singleLineHeight + 1.5f, rect.width, 1.2f));
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Online AIs");
            if (GUI.Button(new Rect(rect.xMax - 20, rect.y, 15, 15), "Refresh"))
            {
                UnityTask.Run(async () =>
                {
                    try
                    {
                        var files = await CloudFileWatcher.Access.GetFiles(SettingsDrawer.UserID);
                        _fileList.Clear();
                        foreach (var file in files)
                        {
                            _fileList.Add(file.Title);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.Message);
                    }
                });
            }
        }

        /// <summary>
        /// Draws this class on the Unity UI.
        /// </summary>
        internal void Draw()
        {
            _list.DoLayoutList();
        }
    }
}