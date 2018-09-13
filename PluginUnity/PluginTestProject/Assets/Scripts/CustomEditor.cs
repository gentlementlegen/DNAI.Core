//using DNAI.MoreOrLess;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;

//namespace Assets.Scripts
//{
//    [CustomEditor(typeof(Play))]
//    public class ListExampleInspector : UnityEditor.Editor
//    {
//        private ReorderableList reorderableList;

//        private Play listExample
//        {
//            get
//            {
//                return target as Play;
//            }
//        }

//        private void OnEnable()
//        {
//            Debug.Log("target => " + target);
//            reorderableList = new ReorderableList(listExample._cdtList, typeof(ConditionItem), true, true, true, true);

//            reorderableList.drawHeaderCallback += DrawHeader;
//            reorderableList.drawElementCallback += DrawElement;

//            reorderableList.onAddCallback += AddItem;
//            reorderableList.onRemoveCallback += RemoveItem;
//            Debug.Log("On enable");
//        }

//        private void OnDisable()
//        {
//            reorderableList.drawHeaderCallback -= DrawHeader;
//            reorderableList.drawElementCallback -= DrawElement;

//            reorderableList.onAddCallback -= AddItem;
//            reorderableList.onRemoveCallback -= RemoveItem;
//        }

//        private void DrawHeader(Rect rect)
//        {
//            GUI.Label(rect, "Our fancy reorderable list LOLILOL");
//        }

//        private void DrawElement(Rect rect, int index, bool active, bool focused)
//        {
//            ConditionItem item = listExample._cdtList[index];

//            EditorGUI.BeginChangeCheck();
//            item.Test = EditorGUI.TextField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), item.Test);
//            if (EditorGUI.EndChangeCheck())
//            {
//                EditorUtility.SetDirty(target);
//            }
//        }

//        private void AddItem(ReorderableList list)
//        {
//            listExample._cdtList.Add(new ConditionItem());

//            EditorUtility.SetDirty(target);
//        }

//        private void RemoveItem(ReorderableList list)
//        {
//            listExample._cdtList.RemoveAt(list.index);

//            EditorUtility.SetDirty(target);
//        }

//        public override void OnInspectorGUI()
//        {
//            Debug.Log("I am here");
//            base.OnInspectorGUI();
//            reorderableList.DoLayoutList();
//        }
//    }
//}