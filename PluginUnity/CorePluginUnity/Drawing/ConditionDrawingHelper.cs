using Core.Plugin.Unity.Runtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Drawing
{
    /// <summary>
    /// Callback for drawing actions in the inspector.
    /// </summary>
    /// <param name="rect">The last rect drawned by the inspector.</param>
    /// <param name="selectedIdx">The current selected index.</param>
    /// <returns></returns>
    public delegate float DrawingAction(Rect rect, AConditionRuntime cdt);

    /// <summary>
    /// Helper to draw DNAI conditions to the Unity Inspector window.
    /// </summary>
    public static class ConditionDrawingHelper
    {
        private static readonly string[] optionsNumber = new string[] { "No condition", "More than", "Less than", "Equal to", "Different than" };
        private static readonly string[] optionsString = new string[] { "No condition", "Equal to", "Different than" };

        private static readonly Dictionary<string, DrawingAction> _drawingActions = new Dictionary<string, DrawingAction>();

        static ConditionDrawingHelper()
        {
            _drawingActions.Add(typeof(Int64).ToString(), new DrawingAction((Rect rect, AConditionRuntime cdt) =>
            {
                var mid = rect.width / 2f;
                cdt._selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), cdt._selectedIdx, optionsNumber);
                if (cdt._selectedIdx != 0)
                    cdt.InputInt = EditorGUI.IntField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), cdt.InputInt);
                return 15;
            }));
            _drawingActions.Add(typeof(int).ToString(), new DrawingAction((Rect rect, AConditionRuntime cdt) => _drawingActions[typeof(Int64).ToString()].Invoke(rect, cdt)));
            _drawingActions.Add(typeof(float).ToString(), new DrawingAction((Rect rect, AConditionRuntime cdt) =>
            {
                var mid = rect.width / 2f;
                //Debug.Log("float => " + mid);
                cdt._selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), cdt._selectedIdx, optionsNumber);
                if (cdt._selectedIdx != 0)
                    cdt.InputFloat = EditorGUI.FloatField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), cdt.InputFloat);
                return 15;
            }));
            _drawingActions.Add(typeof(double).ToString(), new DrawingAction((Rect rect, AConditionRuntime cdt) => _drawingActions[typeof(float).ToString()].Invoke(rect, cdt)));
            _drawingActions.Add(typeof(string).ToString(), new DrawingAction((Rect rect, AConditionRuntime cdt) =>
            {
                var mid = rect.width / 2f;
                //Debug.Log("string");
                cdt._selectedIdx = EditorGUI.Popup(new Rect(rect.x, rect.y, mid, 15), cdt._selectedIdx, optionsString);
                if (cdt._selectedIdx != 0)
                    cdt.InputString = EditorGUI.TextField(new Rect(rect.x + rect.width / 2f + 5, rect.y, mid - 25f, 15), cdt.InputString);
                return 15;
            }));
        }

        /// <summary>
        /// Draws the given condition inside the inspector.
        /// </summary>
        /// <param name="cdtItem">The condition to be drawn</param>
        /// <returns>The size of the drawn object.</returns>
        public static float Draw(ConditionItem cdtItem, Rect rect)
        {
            if (!string.IsNullOrEmpty(cdtItem.cdt.CurrentTypeStr) && cdtItem.SelectedIndex > 0)
                return _drawingActions[cdtItem.cdt.CurrentTypeStr].Invoke(rect, cdtItem.cdt);
            return 0;
        }

        /// <summary>
        /// Gets the drawing size of a condition item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static float GetItemSize(ConditionItem item)
        {
            // + 15 stands for the draw size currently returned by the drawing callbacks
            return 110f + ((item.CallbackCount > 1) ? (item.CallbackCount - 1) * 45f : 0f) + (item.SelectedIndex > 0 ? 15 : 0);
        }
    }
}