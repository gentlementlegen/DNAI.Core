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
        private GUIContent iconToolbarPlus;
        private readonly List<API.File> _fileList = new List<API.File>();

        private static Texture _refreshTexture;

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
            //FetchFiles();
        }

        /// <summary>
        /// Retrieves the files from the server.
        /// </summary>
        public void FetchFiles()
        {
            _fileList.Clear();
            UnityTask.Run(async () =>
            {
                try
                {
                    foreach (var file in await CloudFileWatcher.Access.GetFiles(SettingsDrawer.UserID))
                        _fileList.Add(file);
                }
                catch (NullReferenceException)
                {
                    // Happens when the user is not connected
                    Debug.LogWarning("Cannot fetch files: the DNAI user is not connected.");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error while fetching files: " + ex.Message);
                }
            });
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            Rect labelRect = new Rect(rect.x, rect.y, rect.xMax - 45f, 15f);
            Rect plusRect = new Rect(rect.xMax - 8f - 25f, rect.y, 25f, 13f);

            string str = _fileList[index].Title;
            GUI.Label(labelRect, str);

            if (GUI.Button(plusRect, iconToolbarPlus, preButton))
            {
                UnityTask.Run(async () =>
                {
                    try
                    {
                        bool res = await CloudFileWatcher.DownloadFileAsync(SettingsDrawer.UserID, _fileList[index]);
                        if (!res)
                            Debug.LogWarning($"Could not download the file [{_fileList[index].Title}]");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Exception thrown while downloading the file: " + e.Message + "\nInner Message: " + e.InnerException?.Message);
                    }
                });
            }

            if (index + 1 < _list.count)
                DrawingHelper.Separator(new Rect(labelRect.x, labelRect.y + EditorGUIUtility.singleLineHeight + 1.5f, rect.width, 1.2f));
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Online AIs");
            if (_refreshTexture == null)
                _refreshTexture = AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "refresh.png");
            GUIContent gc = new GUIContent(_refreshTexture, "Refresh");
            GUIStyle skin = new GUIStyle();
            if (GUI.Button(new Rect(rect.xMax - 15, rect.y + 1, 14, 14), gc, skin))
            {
                FetchFiles();
            }
        }

        /// <summary>
        /// Draws this class on the Unity UI.
        /// </summary>
        internal void Draw()
        {
            if (iconToolbarPlus == null)
                iconToolbarPlus = EditorGUIUtility.IconContent("Toolbar Plus", "|Download script");
            if (preButton == null)
                preButton = "RL FooterButton";

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.MaxWidth(Screen.width - 10));
            _list.DoLayoutList();
            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
}