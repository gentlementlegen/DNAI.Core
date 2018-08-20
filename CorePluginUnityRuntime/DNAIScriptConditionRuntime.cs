using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Plugin.Unity.Runtime
{
    public class EventOutputChange : EventArgs
    {
        public DNAIScriptConditionRuntime Invoker;
        public object Value;
        public Type ValueType;
    }

    [Serializable]
    public class UnityEventOutputChange : UnityEvent<EventOutputChange>
    {
    }

    /// <summary>
    /// Base class for every DNAI script.
    /// Separated from the drawing classes, this class is mainly for
    /// runtime purposes.
    /// </summary>
    public class DNAIScriptConditionRuntime : MonoBehaviour
    {
        [HideInInspector]
        public List<ConditionItem> _cdtList = new List<ConditionItem>();
    }

    [System.Serializable]
    public class ConditionItem /*: ScriptableObject*/
    {
        [SerializeField]
        public AConditionRuntime cdt;

        public string Test;

        public static readonly string[] Outputs = new string[]
        {
            "No Output Selected"
            // TODO register outputs
		};

        public UnityEventOutputChange OnOutputChanged;
        public int CallbackCount = 0;

        private float drawSize = 0;

        public float ItemSize
        {
            get { return 110f + ((CallbackCount > 1) ? (CallbackCount - 1) * 45f : 0f) + drawSize; }
        }

        [SerializeField]
        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != _selectedIndex)
                {
                    _selectedIndex = value;
                    cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);
                    //cdt.SetRefOutput(SelectedOutput[value]);
                }
            }
        }

        public string SelectedOutput { get { return Outputs[SelectedIndex]; } }

        public void Initialize(Type t)
        {
            //cdt = new AConditionRuntime();
            cdt = Activator.CreateInstance(t) as AConditionRuntime;
            cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);

            //            <# foreach (var item in EnumNames)
            //{#>
            //	cdt.RegisterEnum(typeof(<#=ClassName#>.<#=item#>).AssemblyQualifiedName);
            //<# } #>
        }

        public virtual float Draw(Rect rect)
        {
            //if (cdt != null)
            //if (cdt.CurrentType == null)
            //cdt.SetCurrentType(SelectedOutput.Split(' ')[0]);

            if (_selectedIndex > 0)
                drawSize = (cdt as ICondition).Draw(rect);
            return drawSize;
        }

        public bool Evaluate<T>(T value)
        {
            //if (cdt != null)
            if (_selectedIndex > 0)
                return cdt.Evaluate(value);
            return true;
        }
    }
}
