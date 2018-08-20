//using System;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine;
//using UnityEngine.Events;

//namespace Assets.Scripts.Test
//{
//    public class EventOutputChange : EventArgs
//    {
//        public object Value;
//        public Type ValueType;
//    }

//    public class DrawEvent : MonoBehaviour
//    {
//        //[HideInInspector]
//        //public string toto;

//        //[HideInInspector]
//        //public UnityEventOutputChange Output;
//        //[HideInInspector]
//        public List<ConditionItem> _cdtList = new List<ConditionItem>();// { new ConditionItem() { cdt = new IntCondition() } };
//    }

//    [CustomEditor(typeof(DrawEvent), true)]
//    public class DrawInEditor : Editor
//    {
//        private ReorderableList reorderableList;
//        private int _selectedIndex;

//        private DrawEvent listExample
//        {
//            get
//            {
//                return target as DrawEvent;
//            }
//        }

//        private void OnEnable()
//        {
//            reorderableList = new ReorderableList(listExample._cdtList, typeof(ConditionItem), true, true, true, true);

//            reorderableList.drawHeaderCallback += DrawHeader;
//            reorderableList.drawElementCallback += DrawElement;

//            reorderableList.onAddCallback += AddItem;
//            reorderableList.onRemoveCallback += RemoveItem;

//            reorderableList.elementHeightCallback += ElementHeightCallback;
//        }

//        private void OnDisable()
//        {
//            reorderableList.drawHeaderCallback -= DrawHeader;
//            reorderableList.drawElementCallback -= DrawElement;

//            reorderableList.onAddCallback -= AddItem;
//            reorderableList.onRemoveCallback -= RemoveItem;

//            reorderableList.elementHeightCallback -= ElementHeightCallback;
//        }

//        private void DrawHeader(Rect rect)
//        {
//            GUI.Label(rect, "Callbacks invoked when output changes");
//        }

//        private void DrawElement(Rect rect, int index, bool active, bool focused)
//        {
//            ConditionItem item = listExample._cdtList[index];
//            Rect newRect = rect;

//            //var s = serializedObject;
//            //s.Update();

//            EditorGUI.BeginChangeCheck();
//            newRect.y += 20;
//            newRect.x += 18;
//            //item.Test = EditorGUI.TextField(new Rect(rect.x + 18, rect.y, rect.width - 18, rect.height), ConditionItem.Outputs[0].Item1);

//            // Draws the condition item selector
//            item.SelectedIndex = EditorGUI.Popup(new Rect(rect.x + 18, rect.y + 2, rect.width - 18, 20), item.SelectedIndex, ConditionItem.Outputs);

//            newRect.y += item.Draw(newRect);

//            // Draws the callback zone to assign it
//            //SerializedObject s = new SerializedObject(listExample);
//            //var p = s.FindProperty("_cdtList").GetArrayElementAtIndex(index);
//            //EditorGUI.PropertyField(new Rect(rect.x + 18, newRect.y + 5, rect.width - 18, 20), p.FindPropertyRelative("OnOutputChanged"));
//            var p = serializedObject.FindProperty("_cdtList").GetArrayElementAtIndex(index);
//            var it = p.serializedObject.GetIterator();
//            p.Next(true);
//            p.Next(false);
//            p.Next(false);
//            //it.Next(true);
//            //it.Next(true);
//            //it.Next(true);
//            Debug.Log("p serialized => " + (p.GetEndProperty()));
//            //it.Next(true);
//            //it.Next(true);
//            Debug.Log("it name => " + it.propertyPath);
//            //CreateCachedEditor(p.objectReferenceValue, null, ref _editor);
//            //_editor.OnInspectorGUI();
//            //foreach (var toto in it)
//            //{
//            //    Debug.Log("item name => " + (toto as SerializedProperty).name);
//            //}
//            //EditorGUIUtility.LookLikeControls();
//            EditorGUI.PropertyField(new Rect(rect.x + 18, newRect.y + 5, rect.width - 18, 20), p);

//            Debug.Log("p property >>>>>> " + p.GetEnumerator());
//            foreach (var field in p.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
//            {
//                Debug.Log("Private fields => " + field.GetValue(p));
//                if (field.GetValue(p) is SerializedObject)
//                {
//                    var so = field.GetValue(p) as SerializedObject;
//                    foreach (var f in so.GetType().GetFields(System.Reflection.BindingFlags.NonPublic))
//                    {
//                        Debug.Log("Private fields => " + f.Name);
//                    }
//                }
//            }
//            item.CallbackCount = item.OnOutputChanged.GetPersistentEventCount();
//            //foreach (var x in p)
//            //{
//            //    var u = x as SerializedProperty;
//            //    Debug.Log("x ===== " + u.propertyPath);
//            //    if (u.name == "size")
//            //    {
//            //        Debug.Log("found size " + u.intValue);
//            //        item.CallbackCount = u.intValue;
//            //        Repaint();
//            //        break;
//            //    }
//            //}

//            if (EditorGUI.EndChangeCheck())
//            {
//                //Debug.Log("end change check true");
//                EditorUtility.SetDirty(target);
//                Debug.Log("End change check lol");
//            }
//            //s.ApplyModifiedPropertiesWithoutUndo();
//        }

//        private Editor _editor;

//        private void AddItem(ReorderableList list)
//        {
//            var item = new ConditionItem();
//            item.Initialize();
//            listExample._cdtList.Add(item);

//            EditorUtility.SetDirty(target);
//        }

//        private void RemoveItem(ReorderableList list)
//        {
//            listExample._cdtList.RemoveAt(list.index);

//            EditorUtility.SetDirty(target);
//        }

//        private float ElementHeightCallback(int idx)
//        {
//            return listExample._cdtList[idx].ItemSize;
//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();
//            //base.OnInspectorGUI();
//            reorderableList.DoLayoutList();
//            serializedObject.ApplyModifiedPropertiesWithoutUndo();
//        }
//    }

//    [Serializable]
//    public class UnityEventOutputChange : UnityEvent<EventOutputChange>
//    {
//    }

//    [CustomPropertyDrawer(typeof(ConditionItem))]
//    public class ConditionItemDraw : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            EditorGUI.LabelField(new Rect(0, 0, 10, 3), "toto !");
//        }
//    }

//    [System.Serializable]
//    public class ConditionItem
//    {
//        [SerializeField]
//        public ACondition cdt;

//        public string Test;

//        public static readonly string[] Outputs = new string[]
//        {
//            "No Output Selected",
//        };

//        public UnityEventOutputChange OnOutputChanged;
//        public int CallbackCount = 0;

//        private float drawSize = 0;

//        public float ItemSize
//        {
//            get { return 110f + ((CallbackCount > 1) ? (CallbackCount - 1) * 45f : 0f) + drawSize; }
//        }

//        [SerializeField]
//        private int _selectedIndex;

//        public int SelectedIndex
//        {
//            get { return _selectedIndex; }
//            set
//            {
//                if (value != _selectedIndex)
//                {
//                    _selectedIndex = value;
//                    cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
//                    //cdt.SetRefOutput(SelectedOutput[value]);
//                }
//            }
//        }

//        public string SelectedOutput { get { return Outputs[SelectedIndex]; } }

//        public void Initialize()
//        {
//            cdt = new ACondition();
//            cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
//        }

//        public float Draw(Rect rect)
//        {
//            //if (cdt != null)
//            //if (cdt.CurrentType == null)
//            //cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
//            if (_selectedIndex > 0)
//                drawSize = cdt.Draw(rect);
//            return drawSize;
//        }

//        public bool Evaluate<T>(T value)
//        {
//            //if (cdt != null)
//            if (_selectedIndex > 0)
//                return cdt.Evaluate(value);
//            return true;
//        }
//    }




//    public delegate void CallbackFunc();

//    /// <summary>
//    /// Callback for drawing actions in the inspector.
//    /// </summary>
//    /// <param name="rect">The last rect drawned by the inspector.</param>
//    /// <param name="selectedIdx">The current selected index.</param>
//    /// <returns></returns>
//    public delegate float DrawingAction(Rect rect, out int selectedIdx);

//    /// <summary>
//    /// Abstract class for conditions related to Unity GUI callbacks.
//    /// Unity does not support Serialization for derived classes,
//    /// so this class handles every time that can be used within the editor.
//    /// </summary>
//    [Serializable]
//    public partial class ACondition : ISerializationCallbackReceiver
//    {

//        [SerializeField]
//        private int _selectedIdx;

//        [SerializeField]
//        private List<string> _registeredTypes = new List<string>();

//        #region Number Options
//        [SerializeField]
//        public enum CONDITION_NUMBER { NO_CONDITION, MORE, LESS, EQUAL, DIFFERENT }
//        private string[] optionsNumber = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };
//        [SerializeField]
//        #endregion

//        #region String Options
//        public enum CONDITION_STRING { NO_CONDITION, EQUAL, DIFFERENT }
//        private string[] optionsString = new string[] { "No condition", "Equal to", "Different than" };

//        #endregion

//        #region Input Serialized Values
//        public int InputInt;
//        public float InputFloat;
//        public string InputString;
//        public string InputEnum;
//        #endregion

//        /// <summary>
//        /// List of drawing actions for given types.
//        /// </summary>
//        private readonly Dictionary<string, DrawingAction> _drawingActions = new Dictionary<string, DrawingAction>();
//        private readonly Dictionary<string, Func<object, bool>> _evaluateActions = new Dictionary<string, Func<object, bool>>();

//        public CallbackFunc Callback;

//        //public Type CurrentType => _currentType;

//        [SerializeField]
//        private string _currentTypeStr;// = "System.Int64";
//        //private Type _currentType;// = typeof(int);

//        public ACondition()
//        {
//            //Debug.Log("Ctor [" + _currentTypeStr + "]");
//            //if (!string.IsNullOrEmpty(_currentTypeStr))
//            //{
//            //    Debug.Log("str is not null or empty");
//            //    _currentType = Type.GetType(_currentTypeStr);
//            //}

//            #region Drawing Actions
//            _drawingActions.Add(typeof(Int64).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
//            {
//                var mid = rect.width / 2f;
//                //Debug.Log("int");
//                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
//                if (_selectedIdx != 0)
//                    InputInt = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputInt);
//                return 15;
//            }));
//            _drawingActions.Add(typeof(int).ToString(), new DrawingAction((Rect rect, out int selectedIndex) => _drawingActions[typeof(Int64).ToString()].Invoke(rect, out selectedIndex)));
//            _drawingActions.Add(typeof(float).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
//            {
//                var mid = rect.width / 2f;
//                //Debug.Log("float => " + mid);
//                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsNumber);
//                if (_selectedIdx != 0)
//                    InputFloat = EditorGUI.FloatField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputFloat);
//                return 15;
//            }));
//            _drawingActions.Add(typeof(double).ToString(), new DrawingAction((Rect rect, out int selectedIndex) => _drawingActions[typeof(float).ToString()].Invoke(rect, out selectedIndex)));
//            _drawingActions.Add(typeof(string).ToString(), new DrawingAction((Rect rect, out int selectedIndex) =>
//            {
//                var mid = rect.width / 2f;
//                //Debug.Log("string");
//                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsString);
//                if (_selectedIdx != 0)
//                    InputString = EditorGUI.TextField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), InputString);
//                return 15;
//            }));
//            #endregion

//            #region Evaluate Actions
//            _evaluateActions.Add(typeof(Int64).ToString(), new Func<object, bool>((obj) =>
//            {
//                var ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
//                Debug.Log("Evaluate int : " + ConditionNumber + " Input=" + InputInt + " Value=" + obj);
//                switch (ConditionNumber)
//                {
//                    case CONDITION_NUMBER.NO_CONDITION:
//                        return true;

//                    case CONDITION_NUMBER.MORE:
//                        return (Int64)obj > InputInt;

//                    case CONDITION_NUMBER.LESS:
//                        return (Int64)obj < InputInt;

//                    case CONDITION_NUMBER.EQUAL:
//                        return (Int64)obj == InputInt;

//                    case CONDITION_NUMBER.DIFFERENT:
//                        return (Int64)obj != InputInt;
//                }
//                return false;
//            }));
//            _evaluateActions.Add(typeof(int).ToString(), new Func<object, bool>((obj) => _evaluateActions[typeof(Int64).ToString()].Invoke(obj)));
//            _evaluateActions.Add(typeof(string).ToString(), new Func<object, bool>((obj) =>
//            {
//                var ConditionString = (CONDITION_STRING)_selectedIdx;
//                Debug.Log("Evaluate string : " + ConditionString + " Input=" + InputString + " Value=" + obj);
//                switch (ConditionString)
//                {
//                    case CONDITION_STRING.NO_CONDITION:
//                        return true;

//                    case CONDITION_STRING.EQUAL:
//                        return (string)obj == InputString;

//                    case CONDITION_STRING.DIFFERENT:
//                        return (string)obj != InputString;
//                }
//                return false;
//            }));
//            _evaluateActions.Add(typeof(float).ToString(), new Func<object, bool>((obj) =>
//            {
//                var ConditionNumber = (CONDITION_NUMBER)_selectedIdx;
//                Debug.Log("Evaluate float : " + ConditionNumber + " Input=" + InputFloat + " Value=" + obj);
//                switch (ConditionNumber)
//                {
//                    case CONDITION_NUMBER.NO_CONDITION:
//                        return true;

//                    case CONDITION_NUMBER.MORE:
//                        return (float)obj > InputFloat;

//                    case CONDITION_NUMBER.LESS:
//                        return (float)obj < InputFloat;

//                    case CONDITION_NUMBER.EQUAL:
//                        return (float)obj == InputFloat;

//                    case CONDITION_NUMBER.DIFFERENT:
//                        return (float)obj != InputFloat;
//                }
//                return false;
//            }));
//            _evaluateActions.Add(typeof(double).ToString(), new Func<object, bool>((obj) => _evaluateActions[typeof(float).ToString()].Invoke(obj)));
//            #endregion
//        }

//        /// <summary>
//        /// Adds a drawing action to the list.
//        /// </summary>
//        public void AddDrawAction(string type, DrawingAction action)
//        {
//            _drawingActions.Add(type, action);
//        }

//        /// <summary>
//        /// Registers an enum to the list of handled types.
//        /// </summary>
//        /// <param name="enumType"></param>
//        public void RegisterEnum(string enumType)
//        {
//            var enumName = enumType.Split(',')[0];
//            Debug.Log("Registering enumeration type =======> " + enumType + " enum name => " + enumName);
//            if (!_registeredTypes.Contains(enumType))
//            {
//                _registeredTypes.Add(enumType);
//            }
//            _drawingActions.Add(enumName, new DrawingAction((Rect rect, out int selectedIndex) =>
//            {
//                var t = Type.GetType(enumType);
//                var mid = rect.width / 2f;
//                //Debug.Log("enum");
//                selectedIndex = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), _selectedIdx, optionsString);
//                if (string.IsNullOrEmpty(InputEnum))
//                {
//                    Debug.Log("Activator is receiving type " + t.ToString());
//                    InputEnum = Activator.CreateInstance(t).ToString();
//                }
//                if (_selectedIdx != 0)
//                    InputEnum = EditorGUI.EnumPopup(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), (Enum)Enum.Parse(t, InputEnum)).ToString();
//                return 15;
//            }));
//            _evaluateActions.Add(enumName, new Func<object, bool>((obj) =>
//            {
//                var ConditionEnum = (CONDITION_STRING)_selectedIdx;
//                Debug.Log("Evaluate enum : " + ConditionEnum + " Input=" + InputEnum + " Value=" + obj);
//                switch (ConditionEnum)
//                {
//                    case CONDITION_STRING.NO_CONDITION:
//                        return true;

//                    case CONDITION_STRING.EQUAL:
//                        return (string)obj == InputEnum;

//                    case CONDITION_STRING.DIFFERENT:
//                        return (string)obj != InputEnum;
//                }
//                return false;
//            }));
//        }

//        private T ConvertVariableType<T>(object input)
//        {
//            return (T)Convert.ChangeType(input, typeof(T));
//        }

//        /// <summary>
//        /// Sets the current type used, such as string, int...
//        /// </summary>
//        /// <param name="type"></param>
//        public void SetCurrentType(string type)
//        {
//            Debug.Log("Set current type to => " + type);
//            _currentTypeStr = type;
//            //_currentType = Type.GetType(_currentTypeStr);
//            _selectedIdx = 0;
//        }

//        /// <summary>
//        /// Evaluates if the condition is satisfied.
//        /// </summary>
//        /// <returns></returns>
//        public virtual bool Evaluate<T>(T val)
//        {
//            //if (_currentType == null && !string.IsNullOrEmpty(_currentTypeStr))
//            //    _currentType = Type.GetType(_currentTypeStr);
//            //Debug.Log("1. Evaluate with type => " + _currentType);
//            Debug.Log("2. Evaluate with type => " + _evaluateActions[_currentTypeStr]);
//            return _evaluateActions[_currentTypeStr].Invoke(val);
//        }

//        /// <summary>
//        /// Draws the condition and return its size.
//        /// </summary>
//        /// <returns></returns>
//        public virtual float Draw(UnityEngine.Rect rect)
//        {
//            Debug.Log("1. Drawing => " + _currentTypeStr);
//            //if (_currentType == null && !string.IsNullOrEmpty(_currentTypeStr))
//            //    _currentType = Type.GetType(_currentTypeStr);
//            //Debug.Log("2. Drawing => " + Type.GetType(_currentTypeStr));
//            //if (_currentType != null)
//            if (!string.IsNullOrEmpty(_currentTypeStr))
//                return _drawingActions[_currentTypeStr].Invoke(rect, out _selectedIdx);
//            return 0;
//        }

//        public void OnBeforeSerialize()
//        {
//        }

//        public void OnAfterDeserialize()
//        {
//        }
//    }
//}