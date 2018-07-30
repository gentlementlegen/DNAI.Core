using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Plugin.Unity.Editor.Conditions.Inspector
{
    /// <summary>
    /// This class can be serialized in the inspector by Unity,
    /// to be able to show the list of conditions available for a script.
    /// </summary>
    /// See also : https://answers.unity.com/questions/826062/re-orderable-object-lists-in-inspector.html
    [System.Serializable]
    public class Play : MonoBehaviour
    {
        public readonly List<ConditionItem> _cdtList = new List<ConditionItem>();
        public int Testing;
    }

    [System.Serializable]
    public class ConditionItem
    {
        private ACondition cdt;

        public string Test;
        public float ItemSize
        {
            get { return 10f; }
        }

        public string AssociatedVariable { get; internal set; }

        public UnityEvent Callback;
    }

    [CustomEditor(typeof(Play))]
    public class ListExampleInspector : UnityEditor.Editor
    {
        private ReorderableList reorderableList;

        private Play listExample
        {
            get
            {
                return target as Play;
            }
        }

        private void OnEnable()
        {
            reorderableList = new ReorderableList(listExample._cdtList, typeof(ConditionItem), true, true, true, true);

            // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
            // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
            // which is a UnityEngine.Object
            // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

            // Add listeners to draw events
            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;

            reorderableList.onAddCallback += AddItem;
            reorderableList.onRemoveCallback += RemoveItem;

            reorderableList.elementHeightCallback += ElementHeightCallback;
        }

        private float ElementHeightCallback(int idx)
        {
            return listExample._cdtList[idx].ItemSize;
        }

        private void OnDisable()
        {
            // Make sure we don't get memory leaks etc.
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;

            reorderableList.onAddCallback -= AddItem;
            reorderableList.onRemoveCallback -= RemoveItem;

            reorderableList.elementHeightCallback -= ElementHeightCallback;
        }

        /// <summary>
        /// Draws the header of the list
        /// </summary>
        /// <param name="rect"></param>
        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Our fancy reorderable list");
        }

        private static readonly string[] outputs = new string[]
            {
                "int myInt"
            };

        /// <summary>
        /// Draws one element of the list (ListItemExample)
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        /// <param name="focused"></param>
        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            ConditionItem item = listExample._cdtList[index];

            EditorGUI.BeginChangeCheck();
            //item.boolValue = EditorGUI.Toggle(new Rect(rect.x, rect.y, 18, rect.height), item.boolValue);
            item.Test = EditorGUI.TextField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item.Test);
            var genericMenu = new GenericMenu();
            EditorGUI.Popup(rect, 0, outputs);

            // https://answers.unity.com/questions/969563/custom-inspector-unity-events.html
            SerializedObject s = new SerializedObject(listExample);
            var p = s.FindProperty("_cdtList").GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, p.FindPropertyRelative("Callback"));

            listExample._cdtList.FindAll((x) => x.AssociatedVariable == nameof(item)).ForEach((y) => y.Callback?.Invoke());

            Object obj = null;
            EditorGUI.ObjectField(rect, obj, typeof(Object), true);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            // If you are using a custom PropertyDrawer, this is probably better
            // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
            // Although it is probably smart to cach the list as a private variable ;)
        }

        private void AddItem(ReorderableList list)
        {
            listExample._cdtList.Add(new ConditionItem());

            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            listExample._cdtList.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // Actually draw the list in the inspector
            reorderableList.DoLayoutList();
        }
    }
}