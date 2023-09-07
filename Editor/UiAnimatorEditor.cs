using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UiAnimation
{
#if UNITY_EDITOR
	[CustomEditor( typeof( UiAnimator ) )]
	public class UiAnimatorEditor : Editor
	{
		private const int DefaultArgumetnsCount = 8;
		private const string NotSet = "NOT SET!!!";

		UiAnimator animator;


		private void OnEnable( )
		{
			animator = ( UiAnimator ) target;
		}

		public override void OnInspectorGUI( )
		{
			serializedObject.Update( );
			
			var allowInit = serializedObject.FindProperty( "AllowInit" );
			allowInit.boolValue = EditorGUILayout.Toggle( "Allow Init", allowInit.boolValue );
			DrawContent( );
			EditorGUILayout.Space( );
			EditorGUILayout.Space( );
			DrawAddButton( );
			serializedObject.ApplyModifiedProperties( );

		}

		public void OnInspectorUpdate( )
		{
			this.Repaint( );
		}

		private void DrawContent( )
		{
			GUIStyle style = new GUIStyle( GUI.skin.label );
			style.fontStyle = FontStyle.Bold;
			var items = animator.Items;
			if ( items == null ) return;
			for ( int i = 0; i < items.Count; i++ )
			{
				var item = serializedObject.FindProperty( "items" ).GetArrayElementAtIndex( i );
				var foldout = item.FindPropertyRelative( "foldout" );

				string[] options = Enum.GetNames( typeof( EAnimationType ) );
				string[] easings = Enum.GetNames( typeof( LeanTweenType ) );

				var selected = item.FindPropertyRelative( "selected" );
				string status = $"{( EAnimationType ) selected.intValue} Animation";
				SerializedProperty move = item.FindPropertyRelative( "moveAnimator" );
				SerializedProperty alpha = item.FindPropertyRelative( "alphaAnimator" );
				SerializedProperty size = item.FindPropertyRelative( "sizeAnimator" );
				SerializedProperty scale = item.FindPropertyRelative( "scaleAnimator" );

				List<SerializedProperty> moveProperties = new List<SerializedProperty>( );
				List<SerializedProperty> alphaProperties = new List<SerializedProperty>( );
				List<SerializedProperty> sizeProperties = new List<SerializedProperty>( );
				List<SerializedProperty> scaleProperties = new List<SerializedProperty>( );

				// isActive         0
				// autoPlay         1
				// appearEasing     2
				// disappearEasing  3
				// delayIn          4
				// delayOut         5
				// animTime         6
				// isInverse        7

				FillCommonList( moveProperties, move );
				FillCommonList( alphaProperties, alpha );
				FillCommonList( sizeProperties, size );
				FillCommonList( scaleProperties, scale );

				moveProperties.Add( move.FindPropertyRelative( "xOffset" ) );
				moveProperties.Add( move.FindPropertyRelative( "yOffset" ) );
				moveProperties.Add( move.FindPropertyRelative( "moveObject" ) );
				moveProperties.Add( move.FindPropertyRelative( "isLocal" ) );
				moveProperties.Add( move.FindPropertyRelative( "moveType" ) );
				moveProperties.Add( move.FindPropertyRelative( "xOffsetUnits" ) );
				moveProperties.Add( move.FindPropertyRelative( "yOffsetUnits" ) );

				alphaProperties.Add( alpha.FindPropertyRelative( "from" ) );
				alphaProperties.Add( alpha.FindPropertyRelative( "to" ) );
				alphaProperties.Add( alpha.FindPropertyRelative( "cg" ) );
				alphaProperties.Add( alpha.FindPropertyRelative( "autoOff" ) );

				sizeProperties.Add( size.FindPropertyRelative( "from" ) );
				sizeProperties.Add( size.FindPropertyRelative( "to" ) );
				sizeProperties.Add( size.FindPropertyRelative( "rect" ) );

				scaleProperties.Add( scale.FindPropertyRelative( "from" ) );
				scaleProperties.Add( scale.FindPropertyRelative( "to" ) );
				scaleProperties.Add( scale.FindPropertyRelative( "gameObject" ) );

				List<SerializedProperty> selectedList = new List<SerializedProperty>( );

				switch ( ( EAnimationType ) selected.intValue )
				{
					case EAnimationType.Move: selectedList = moveProperties; break;
					case EAnimationType.Alpha: selectedList = alphaProperties; break;
					case EAnimationType.Scale: selectedList = scaleProperties; break;
					case EAnimationType.Size: selectedList = sizeProperties; break;
				}

				if ( selected.intValue == 0 )
					status += moveProperties[DefaultArgumetnsCount + 3].objectReferenceValue == null ? $" -> {NotSet}" : $" -> {moveProperties[DefaultArgumetnsCount + 3].objectReferenceValue?.name}";
				if ( selected.intValue == 1 )
					status += alphaProperties[DefaultArgumetnsCount + 3].objectReferenceValue == null ? $" -> {NotSet}" : $" -> {alphaProperties[DefaultArgumetnsCount + 3].objectReferenceValue?.name}";
				if ( selected.intValue == 2 )
					status += scaleProperties[DefaultArgumetnsCount + 3].objectReferenceValue == null ? $" -> {NotSet}" : $" -> {scaleProperties[DefaultArgumetnsCount + 3].objectReferenceValue?.name}";
				if ( selected.intValue == 3 )
					status += sizeProperties[DefaultArgumetnsCount + 3].objectReferenceValue == null ? $" -> {NotSet}" : $" -> {sizeProperties[DefaultArgumetnsCount + 3].objectReferenceValue?.name}";


				EditorGUILayout.BeginHorizontal( );
				GUIStyle styleS = selectedList[0].boolValue ? CustomStyles.WhiteFold : CustomStyles.GrayFold;
				foldout.boolValue = EditorGUILayout.
					BeginFoldoutHeaderGroup( foldout.boolValue, status, styleS );
				DrawActiveButton( selectedList );
				DrawUpButton( i );
				DrawDownButton( i );

				DrawDeleteButton( i );
				EditorGUILayout.EndHorizontal( );
				if ( foldout.boolValue )
				{
					if ( Selection.activeTransform )
					{
						EditorGUI.indentLevel++;
						EditorGUILayout.BeginVertical( CustomStyles.OutlinedBox );
						EditorGUILayout.Space( );
						selected.intValue = EditorGUILayout.Popup( "Type", selected.intValue, options );
						selectedList[1].boolValue = EditorGUILayout.Toggle( "AutoPlay", selectedList[1].boolValue );
						selectedList[7].boolValue = EditorGUILayout.Toggle( "IsInverse", selectedList[7].boolValue );
						selectedList[8].boolValue = EditorGUILayout.Toggle( "Reset On Play", selectedList[8].boolValue );
						EditorGUILayout.Space( );
						if ( selected.intValue == 0 ) DrawMove( moveProperties );
						if ( selected.intValue == 1 ) DrawAlpha( alphaProperties );
						if ( selected.intValue == 2 ) DrawScale( scaleProperties );
						if ( selected.intValue == 3 ) DrawSize( sizeProperties );

						selectedList[6].floatValue = EditorGUILayout.FloatField(
							"Play Time", selectedList[6].floatValue );

						EditorGUILayout.Space( );
						EditorGUILayout.LabelField( $"Delays", style );
						selectedList[4].floatValue = EditorGUILayout.FloatField(
							"Delay In", selectedList[4].floatValue );

						selectedList[5].floatValue = EditorGUILayout.FloatField(
							"Delay Out", selectedList[5].floatValue );
						EditorGUILayout.Space( );

						EditorGUILayout.LabelField( $"Easings", style );
						selectedList[2].intValue = EditorGUILayout.
							Popup( "Appear Easing", selectedList[2].intValue, easings );
						selectedList[3].intValue = EditorGUILayout.
							Popup( "Disappear Easing", selectedList[3].intValue, easings );
						EditorGUI.indentLevel--;
						EditorGUILayout.Space( );
						EditorGUILayout.EndVertical( );
					}
				}

				EditorGUILayout.EndFoldoutHeaderGroup( );
			}
		}

		private void FillCommonList( List<SerializedProperty> properties, SerializedProperty target )
		{
			properties.Clear( );
			properties.Add( target.FindPropertyRelative( "isActive" ) );
			properties.Add( target.FindPropertyRelative( "autoPlay" ) );
			properties.Add( target.FindPropertyRelative( "appearEasing" ) );
			properties.Add( target.FindPropertyRelative( "dissapearEasing" ) );
			properties.Add( target.FindPropertyRelative( "inDelay" ) );
			properties.Add( target.FindPropertyRelative( "outDelay" ) );
			properties.Add( target.FindPropertyRelative( "animationTime" ) );
			properties.Add( target.FindPropertyRelative( "isInverse" ) );
			properties.Add( target.FindPropertyRelative( "resetOnStart" ) );
		}

		private void DrawMove( List<SerializedProperty> properties )
		{
			string[] options = Enum.GetNames( typeof( EMoveType ) );
			properties[DefaultArgumetnsCount + 5].intValue = EditorGUILayout.Popup( "Type", properties[DefaultArgumetnsCount + 5].intValue, options );

			properties[DefaultArgumetnsCount + 4].boolValue = EditorGUILayout.Toggle( "Is Local", properties[DefaultArgumetnsCount + 4].boolValue );
			properties[DefaultArgumetnsCount + 3].objectReferenceValue = EditorGUILayout.
				ObjectField( "Move Object", properties[DefaultArgumetnsCount + 3].objectReferenceValue, typeof( GameObject ), true );
			if ( properties[DefaultArgumetnsCount + 5].intValue == 1 )
			{

				properties[DefaultArgumetnsCount + 1].floatValue = EditorGUILayout.Slider(
							"X Offset %",
							properties[DefaultArgumetnsCount + 1].floatValue, -1, 1 );
				properties[DefaultArgumetnsCount + 2].floatValue = EditorGUILayout.Slider(
											"Y Offset %",
											properties[DefaultArgumetnsCount + 2].floatValue, -1, 1 );
			}
			else
			{
				properties[DefaultArgumetnsCount + 6].floatValue = EditorGUILayout.FloatField(
							"X Offset Units",
							properties[DefaultArgumetnsCount + 6].floatValue );
				properties[DefaultArgumetnsCount + 7].floatValue = EditorGUILayout.FloatField(
											"Y Offset Units",
											properties[DefaultArgumetnsCount + 7].floatValue );
			}
		}

		private void DrawAlpha( List<SerializedProperty> properties )
		{
			properties[DefaultArgumetnsCount + 4].boolValue = EditorGUILayout.Toggle( "Off Cg After", properties[DefaultArgumetnsCount + 4].boolValue );
			properties[DefaultArgumetnsCount + 3].objectReferenceValue = ( CanvasGroup ) EditorGUILayout.
					ObjectField( "Canvas Group", properties[DefaultArgumetnsCount + 3].objectReferenceValue, typeof( CanvasGroup ), true );
			properties[DefaultArgumetnsCount + 1].floatValue = EditorGUILayout.Slider(
										"From Alpha",
										properties[DefaultArgumetnsCount + 1].floatValue, 0, 1 );
			properties[DefaultArgumetnsCount + 2].floatValue = EditorGUILayout.Slider(
										"To Alpha",
										properties[DefaultArgumetnsCount + 2].floatValue, 0, 1 );
		}

		private void DrawSize( List<SerializedProperty> properties )
		{
			properties[DefaultArgumetnsCount + 3].objectReferenceValue = EditorGUILayout.
				ObjectField( "Siz Object", properties[DefaultArgumetnsCount + 3].objectReferenceValue, typeof( RectTransform ), true );
			properties[DefaultArgumetnsCount + 1].vector2Value = EditorGUILayout.Vector2Field( "From", properties[DefaultArgumetnsCount + 1].vector2Value );
			properties[DefaultArgumetnsCount + 2].vector2Value = EditorGUILayout.Vector2Field( "To", properties[DefaultArgumetnsCount + 2].vector2Value );
		}

		private void DrawScale( List<SerializedProperty> properties )
		{
			properties[DefaultArgumetnsCount + 3].objectReferenceValue = EditorGUILayout.
				ObjectField( "Scale Object", properties[DefaultArgumetnsCount + 3].objectReferenceValue, typeof( GameObject ), true );

			properties[DefaultArgumetnsCount + 1].vector3Value = EditorGUILayout.Vector3Field( "From", properties[DefaultArgumetnsCount + 1].vector3Value );
			properties[DefaultArgumetnsCount + 2].vector3Value = EditorGUILayout.Vector3Field( "To", properties[DefaultArgumetnsCount + 2].vector3Value );
		}

		private void DrawDeleteButton( int index )
		{
			if ( GUILayout.Button( "-", EditorStyles.miniButtonRight, GUILayout.MaxWidth( 20 ) ) )
			{
				animator.RemoveAt( index );
			}
		}

		private void DrawDownButton( int index )
		{
			if ( GUILayout.Button( "\u21b4", EditorStyles.miniButtonRight, GUILayout.MaxWidth( 20 ) ) )
			{
				animator.Down( index );
			}
		}

		private void DrawUpButton( int index )
		{
			if ( GUILayout.Button( "\u2191", EditorStyles.miniButtonRight, GUILayout.MaxWidth( 20 ) ) )
			{
				animator.Up( index );
			}
		}

		private void DrawActiveButton( List<SerializedProperty> selected )
		{
			string targetText = selected[0].boolValue ? "active" : "not active";
			GUIStyle style = selected[0].boolValue ? CustomStyles.WhiteButton : CustomStyles.GrayButton;
			if ( GUILayout.Button( targetText, style, GUILayout.MaxWidth( 80 ) ) )
			{
				selected[0].boolValue = !selected[0].boolValue;
			}
		}

		private void DrawAddButton( )
		{
			GUILayout.BeginVertical( );
			GUILayout.BeginHorizontal( );
			GUILayout.FlexibleSpace( );
			if ( GUILayout.Button( "Add Animation", GUILayout.MaxWidth( 240 ), GUILayout.Height( 25 ) ) )
			{
				animator.Add( );
			}
			GUILayout.FlexibleSpace( );
			GUILayout.EndHorizontal( );
			GUILayout.EndVertical( );
		}
	}
#endif
}