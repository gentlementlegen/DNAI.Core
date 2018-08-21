using Core.Plugin.Unity.Editor.Conditions;
using Core.Plugin.Unity.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Plugin.Unity.Drawing
{
   // [Serializable]
   // public class UnityEventOutputChange : UnityEvent<EventOutputChange>
   // {
   // }

   // [System.Serializable]
   // public class ConditionItem /*: ScriptableObject*/
   // {
   //     [SerializeField]
   //     public ACondition cdt;

   //     public string Test;

   //     public static readonly string[] Outputs = new string[]
   //     {
   //             "No Output Selected"
			//};

   //     public UnityEventOutputChange OnOutputChanged;
   //     public int CallbackCount = 0;

   //     private float drawSize = 0;

   //     public float ItemSize
   //     {
   //         get { return 110f + ((CallbackCount > 1) ? (CallbackCount - 1) * 45f : 0f) + drawSize; }
   //     }

   //     [SerializeField]
   //     private int _selectedIndex;

   //     public int SelectedIndex
   //     {
   //         get { return _selectedIndex; }
   //         set
   //         {
   //             if (value != _selectedIndex)
   //             {
   //                 _selectedIndex = value;
   //                 cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
   //                 cdt.SetRefOutput(SelectedOutput[value]);
   //             }
   //         }
   //     }

   //     public string SelectedOutput { get { return Outputs[SelectedIndex]; } }

   //     public void Initialize()
   //     {
   //         cdt = new ACondition();
   //         cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);

   //         //            <# foreach (var item in EnumNames)
   //         //{#>
   //         //	cdt.RegisterEnum(typeof(<#=ClassName#>.<#=item#>).AssemblyQualifiedName);
   //         //<# } #>
   //     }

   //     public float Draw(Rect rect)
   //     {
   //         //if (cdt != null)
   //         //if (cdt.CurrentType == null)
   //         //cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
   //         if (_selectedIndex > 0)
   //             drawSize = cdt.Draw(rect);
   //         return drawSize;
   //     }

   //     public bool Evaluate<T>(T value)
   //     {
   //         //if (cdt != null)
   //         if (_selectedIndex > 0)
   //             return cdt.Evaluate(value);
   //         return true;
   //     }
   // }

    [CustomEditor(typeof(DNAIScriptConditionRuntime), true)]
    public class ListExampleInspector : UnityEditor.Editor
    {
        private ReorderableList reorderableList;
        //private int _selectedIndex;

        private DNAIScriptConditionRuntime listExample
        {
            get
            {
                return target as DNAIScriptConditionRuntime;
            }
        }

        private void OnEnable()
        {
            reorderableList = new ReorderableList(listExample._cdtList, typeof(ConditionItem), true, true, true, true);

            reorderableList.drawHeaderCallback += DrawHeader;
            reorderableList.drawElementCallback += DrawElement;

            reorderableList.onAddCallback += AddItem;
            reorderableList.onRemoveCallback += RemoveItem;

            reorderableList.elementHeightCallback += ElementHeightCallback;
        }

        private void OnDisable()
        {
            reorderableList.drawHeaderCallback -= DrawHeader;
            reorderableList.drawElementCallback -= DrawElement;

            reorderableList.onAddCallback -= AddItem;
            reorderableList.onRemoveCallback -= RemoveItem;

            reorderableList.elementHeightCallback -= ElementHeightCallback;
        }

        private void DrawHeader(Rect rect)
        {
            GUI.Label(rect, "Callbacks invoked when output changes");
        }

        private void DrawElement(Rect rect, int index, bool active, bool focused)
        {
            var item = listExample._cdtList[index];
            Rect newRect = rect;

            //var s = serializedObject;
            //s.Update();

            EditorGUI.BeginChangeCheck();
            newRect.y += 20;
            newRect.x += 18;
            //item.Test = EditorGUI.TextField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), ConditionItem.Outputs[0].Item1);

            // Draws the condition item selector
            item.SelectedIndex = EditorGUI.Popup(new Rect(rect.x + 18, rect.y + 2, rect.width - 18, 20), item.SelectedIndex, item.Outputs);

            //Debug.Log("Before drawing y=" + newRect.y);
            //newRect.y += item.Draw(newRect);
            newRect.y += ConditionDrawingHelper.Draw(item, newRect);
            //Debug.Log("After drawing y=" + newRect.y);


            // Draws the callback zone to assign it
            //SerializedObject s = new SerializedObject(listExample);
            var p = serializedObject.FindProperty("_cdtList").GetArrayElementAtIndex(index);
            //EditorGUI.PropertyField(new Rect(rect.x + 18, newRect.y + 5, rect.width - 18, 20), p.FindPropertyRelative("OnOutputChanged"));
            //var p = serializedObject.FindProperty("OnOutputChanged");
            //EditorGUIUtility.LookLikeControls();

            //Debug.Log("p is => " + p);
            var it = p.serializedObject.GetIterator();
            p.Next(true);
            p.Next(false);
            p.Next(false);
            p.Next(false);
            EditorGUI.PropertyField(new Rect(rect.x + 18, newRect.y + 5, rect.width - 18, 20), p);

            item.CallbackCount = item.OnOutputChanged.GetPersistentEventCount();
            //foreach (var x in p)
            //{
            //	var u = x as SerializedProperty;
            //	if (u.name == "size")
            //	{
            //		Debug.Log("found size " + u.intValue);
            //		item.CallbackCount = u.intValue;
            //		Repaint();
            //		break;
            //	}
            //}

            if (EditorGUI.EndChangeCheck())
            {
                //Debug.Log("end change check true");
                EditorUtility.SetDirty(target);
                //Debug.Log("End change check !");
                Repaint();
            }
            //s.ApplyModifiedPropertiesWithoutUndo();
        }

        private void AddItem(ReorderableList list)
        {
            //var item = ScriptableObject.CreateInstance<ConditionItem>();
            var item = new Core.Plugin.Unity.Runtime.ConditionItem
            {
                Outputs = (string[])serializedObject.targetObject.GetType().GetField("OutputsAsStrings", System.Reflection.BindingFlags.FlattenHierarchy
                | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).GetValue(serializedObject.targetObject)
            };
            item.Initialize(typeof(ACondition));
            Debug.Log(serializedObject.targetObject.GetType());

            //Debug.Log("Static field => " + serializedObject.targetObject.GetType().GetField("OutputsAsStrings", System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null));
            //var field = (string[])serializedObject.targetObject.GetType().GetField("OutputsAsStrings", System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(null);
            //Debug.Log(field[0]);
            //var member = serializedObject.targetObject.GetType().GetMember("OutputsAsStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //Debug.Log(serializedObject.targetObject.GetType().GetMember("OutputsAsStrings", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance));
            //foreach (var output in field)
            //{
            //    Debug.Log(output);
            //}
            listExample._cdtList.Add(item);

            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list)
        {
            listExample._cdtList.RemoveAt(list.index);

            EditorUtility.SetDirty(target);
        }

        private float ElementHeightCallback(int idx)
        {
            //return listExample._cdtList[idx].ItemSize;
            return ConditionDrawingHelper.GetItemSize(listExample._cdtList[idx]);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }

    //public class EventOutputChange : EventArgs
    //{
    //    public DNAIScriptConditionDrawer Invoker;
    //    public object Value;
    //    public Type ValueType;
    //}
}