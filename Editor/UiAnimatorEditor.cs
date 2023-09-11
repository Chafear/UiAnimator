using System;
using UnityEditor;
using UnityEngine;

namespace UiAnimation
{
#if UNITY_EDITOR
	[CustomEditor( typeof( UiAnimator ) )]
	public sealed class UiAnimatorEditor : Editor
	{
		private const string NotSet = "NOT SET!!!";

		UiAnimator animator;

		private void OnEnable( )
		{
			animator = ( UiAnimator ) target;
		}

		public override void OnInspectorGUI( )
		{
			serializedObject.Update( );
			EditorGUILayout.PropertyField( serializedObject.FindProperty( "allowInit" ) );
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

				var selected = item.FindPropertyRelative( "selected" );
				string status = $"{( EAnimationType ) selected.intValue} Animation";

				SerializedProperty move = item.FindPropertyRelative( "moveAnimation" );
				SerializedProperty alpha = item.FindPropertyRelative( "alphaAnimation" );
				SerializedProperty size = item.FindPropertyRelative( "sizeAnimation" );
				SerializedProperty scale = item.FindPropertyRelative( "scaleAnimation" );

				SerializedProperty selectedProperty = null;

				EAnimationType animationType = ( EAnimationType ) selected.intValue;

				switch ( animationType )
				{
					case EAnimationType.Move: 
						selectedProperty = move;
						status += CheckStatusIfObjectSet( selectedProperty, "moveObject" );
						break;
					case EAnimationType.Alpha: 
						selectedProperty = alpha;
						status += CheckStatusIfObjectSet( selectedProperty, "canvasGroup" );
						break;
					case EAnimationType.Scale: 
						selectedProperty = scale;
						status += CheckStatusIfObjectSet( selectedProperty, "scaleObject" );
						break;
					case EAnimationType.Size: 
						selectedProperty = size;
						status += CheckStatusIfObjectSet( selectedProperty, "toResize" );
						break;
				}

				EditorGUILayout.BeginHorizontal( );
				bool toFold = foldout.boolValue;
				GUIStyle selectedStyle = toFold ? CustomStyles.WhiteFold : CustomStyles.GrayFold;
				foldout.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup( foldout.boolValue, 
					status, selectedStyle );
				DrawActiveButton( selectedProperty );
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

						EditorGUILayout.PropertyField( item.FindPropertyRelative( "selected" ) );
						EditorGUILayout.Space( );
						
						DrawCommonBeforeUnique( selectedProperty );
						switch ( animationType )
						{
							case EAnimationType.Move: DrawMove( selectedProperty );	break;
							case EAnimationType.Alpha: DrawAlpha( selectedProperty ); break;
							case EAnimationType.Scale: DrawScale( selectedProperty ); break;
							case EAnimationType.Size: DrawSize( selectedProperty ); break;
						}
						DrawCommonAfterUnique( selectedProperty, style );

						
						EditorGUI.indentLevel--;
						EditorGUILayout.Space( );
						EditorGUILayout.EndVertical( );
					}
				}
				EditorGUILayout.EndFoldoutHeaderGroup( );
			}
		}

		private void DrawCommonBeforeUnique( SerializedProperty target )
		{
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "autoPlay" ) );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "isInverse" ) );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "resetOnStart" ) );
		}
		
		private void DrawCommonAfterUnique( SerializedProperty target, GUIStyle style )
		{
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "animationTime" ) );
			EditorGUILayout.Space( );
			EditorGUILayout.LabelField( $"Delays", style );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "inDelay" ) );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "outDelay" ) );
			EditorGUILayout.Space( );
			EditorGUILayout.LabelField( $"Easings", style );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "appearEasing" ) );
			EditorGUILayout.PropertyField( target.FindPropertyRelative( "dissapearEasing" ) );
		}

		private void DrawMove( SerializedProperty move )
		{
			EMoveType moveType = (EMoveType) move.FindPropertyRelative( "moveType" ).intValue;
			EditorGUILayout.PropertyField( move.FindPropertyRelative( "moveType" ) );
			EditorGUILayout.PropertyField( move.FindPropertyRelative( "isLocal" ) );
			EditorGUILayout.PropertyField( move.FindPropertyRelative( "moveObject" ) );
			
			switch ( moveType )
			{
				case EMoveType.Units:
					EditorGUILayout.PropertyField( move.FindPropertyRelative( "xOffsetUnits" ) );
					EditorGUILayout.PropertyField( move.FindPropertyRelative( "yOffsetUnits" ) );
					break;
				case EMoveType.Percent:
					var xOffsetPercent = move.FindPropertyRelative( "xOffset" );
					var yOffsetPercent = move.FindPropertyRelative( "yOffset" );
					xOffsetPercent.floatValue = EditorGUILayout.Slider(
							"X Offset %", xOffsetPercent.floatValue, -1, 1 );
					yOffsetPercent.floatValue = EditorGUILayout.Slider(
							"Y Offset %", yOffsetPercent.floatValue, -1, 1 );
					break;
			}
		}

		private void DrawAlpha( SerializedProperty alpha )
		{
			EditorGUILayout.PropertyField( alpha.FindPropertyRelative( "autoDisableCanvasGroup" ) );
			EditorGUILayout.PropertyField( alpha.FindPropertyRelative( "canvasGroup" ) );
			
			var from = alpha.FindPropertyRelative( "from" );
			var to = alpha.FindPropertyRelative( "to" );

			from.floatValue = EditorGUILayout.Slider( "From Alpha", from.floatValue, 0, 1 );
			to.floatValue = EditorGUILayout.Slider( "To Alpha",	to.floatValue, 0, 1 );
		}

		private void DrawSize( SerializedProperty size )
		{
			EditorGUILayout.PropertyField( size.FindPropertyRelative( "toResize" ) );
			EditorGUILayout.PropertyField( size.FindPropertyRelative( "from" ) );
			EditorGUILayout.PropertyField( size.FindPropertyRelative( "to" ) );
		}

		private void DrawScale( SerializedProperty scale )
		{
			EditorGUILayout.PropertyField( scale.FindPropertyRelative( "scaleObject" ) );
			EditorGUILayout.PropertyField( scale.FindPropertyRelative( "from" ) );
			EditorGUILayout.PropertyField( scale.FindPropertyRelative( "to" ) );
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

		private void DrawActiveButton( SerializedProperty selected )
		{
			SerializedProperty isActive = selected.FindPropertyRelative( "isActive" );
			string targetText = isActive.boolValue ? "active" : "not active";
			GUIStyle style = isActive.boolValue ? CustomStyles.WhiteButton : CustomStyles.GrayButton;
			if ( GUILayout.Button( targetText, style, GUILayout.MaxWidth( 80 ) ) )
			{
				isActive.boolValue = !isActive.boolValue;
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

		private string CheckStatusIfObjectSet( SerializedProperty property, string objectId )
		{
			var reference = property.FindPropertyRelative( objectId ).objectReferenceValue;
			return reference == null ? $" -> {NotSet}" : $" -> {reference?.name}";
		}
	}
#endif
}