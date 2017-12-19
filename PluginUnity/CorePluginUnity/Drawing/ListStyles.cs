using System;
using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Drawing
{
	public static class ListStyles
	{
		static ListStyles ()
		{
			Title = new GUIStyle();
			Title.border = new RectOffset(2, 2, 2, 1);
			Title.margin = new RectOffset(5, 5, 5, 0);
			Title.padding = new RectOffset(5, 5, 3, 3);
			Title.alignment = TextAnchor.MiddleLeft;
			Title.normal.textColor = EditorGUIUtility.isProSkin
				? new Color(0.8f, 0.8f, 0.8f)
				: new Color(0.2f, 0.2f, 0.2f);
		}

		/// <summary>
		/// Gets style for title header.
		/// </summary>
		public static GUIStyle Title { get; private set; }
	}
}

