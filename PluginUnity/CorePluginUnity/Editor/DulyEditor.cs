using Core.Plugin.Drawing;
using UnityEditor;
using UnityEngine;

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

// TODO : L'idée ce serait de faire un système comme les unity events, ou on chargerait un script,
// qui une fois chargé (avec des threads) afficherait les behaviours dispos
// Pour ce qui est de l'affichage, ce serait comme les unity events, mais avec un zone
// de texte pour mettre le path à la place du truc pour selectionner les objets, et au bout un bouton load.
// Si le fichier est bien load, ça montre tous les behaviours disponibles en dessous.

// Note : pour unity, si la .dll référence d'autres .dll cela provoque un crash au runtime. Deux solutions :
// soit copier toutes les .dll dans le dossier plugin, soit utiliser un utilitaire de merge de librairies
// cf : https://github.com/Microsoft/ILMerge/blob/master/ilmerge-manual.md

namespace Core.Plugin.Editor
{
    /// <summary>
    /// Duly editor, the unique, the one.
    /// </summary>
    public class DulyEditor : EditorWindow
    {
        private ScriptDrawer _scriptDrawer;

        [MenuItem("Component/Duly")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(DulyEditor));
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            DrawWindowTitle();
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            _scriptDrawer.Draw();
        }

        private void OnEnable()
        {
            _scriptDrawer = ScriptableObject.CreateInstance<ScriptDrawer>();
        }

        #region Editor Drawing

        private void DrawWindowTitle()
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("Duly Editor V1.0", EditorStyles.largeLabel);
            GUILayout.FlexibleSpace();
        }

        #endregion Editor Drawing
    }
}