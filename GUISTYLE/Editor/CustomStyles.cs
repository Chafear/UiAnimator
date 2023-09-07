using UnityEditor;
using UnityEngine;

namespace UiAnimation
{
#if UNITY_EDITOR
	public static class CustomStyles
	{

		public static GUIStyle OutlinedBox => EditorStyles.helpBox;

		public static GUIStyle WhiteButton
		{
			get
			{
				if ( whiteButton == null )
				{
					whiteButton = new GUIStyle( EditorStyles.miniButton );
					whiteButton.font = EditorStyles.boldFont;
					whiteButton.normal.textColor = Color.white;
				}
				return whiteButton;
			}
		}
		public static GUIStyle GrayButton
		{
			get
			{
				if ( grayButton == null )
				{
					grayButton = new GUIStyle( EditorStyles.miniButton );
					grayButton.font = EditorStyles.boldFont;
					grayButton.normal.textColor = Color.gray;
				}
				return grayButton;
			}
		}
		public static GUIStyle WhiteFold
		{
			get
			{
				if ( whiteFoldout == null )
				{
					whiteFoldout = new GUIStyle( EditorStyles.foldoutHeader );
					whiteFoldout.font = EditorStyles.boldFont;
					whiteFoldout.normal.textColor = Color.white;
				}
				return whiteFoldout;
			}
		}
		public static GUIStyle GrayFold
		{
			get
			{
				if ( grayFoldout == null )
				{
					grayFoldout = new GUIStyle( EditorStyles.foldoutHeader );
					grayFoldout.font = EditorStyles.boldFont;
					grayFoldout.normal.textColor = Color.gray;
				}
				return grayFoldout;
			}
		}

		private static GUIStyle whiteFoldout;
		private static GUIStyle grayFoldout;
		private static GUIStyle whiteButton;
		private static GUIStyle grayButton;
	}
#endif
}

