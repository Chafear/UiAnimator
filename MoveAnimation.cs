using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public sealed class MoveAnimation : UiAnimationBase
	{
		[SerializeField] private bool isLocal = true;
		[SerializeField] private EMoveType moveType = EMoveType.Percent;
		[Range( -1.0f, 1.0f )]
		[SerializeField] private float xOffset = 0;
		[Range( -1.0f, 1.0f )]
		[SerializeField] private float yOffset = 0;
		[SerializeField] private float xOffsetUnits = 0;
		[SerializeField] private float yOffsetUnits = 0;
		[SerializeField] private GameObject moveObject;

		private Vector3 startPos;

		public override void OnInit( )
		{
			if ( moveObject == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			startPos = isLocal ? moveObject.transform.localPosition : moveObject.transform.position;

			if ( !autoPlay ) return;

			if ( !isLocal )
			{
				var xy = GetXY( );
				moveObject.transform.position = startPos
					+ new Vector3( xy.x, xy.y, 0 );
			} else
			{
				var xy = GetXY( );
				moveObject.transform.localPosition = startPos
					+ new Vector3( xy.x, xy.y, 0 );
			}
		}

		public override void OutPlay( bool insta = false )
		{
			CancelAnim( );
			if ( !isLocal )
			{
				var xy = GetXY( );
				Vector3 newPos = startPos
					+ new Vector3( xy.x, xy.y, 0 );

				if ( resetOnStart )
					moveObject.transform.position = startPos;
				if ( insta )
				{
					moveObject.transform.position = newPos;
					return;
				}
				animIds.Add( LeanTween.move( moveObject, newPos, animationTime )
					.setEase( dissapearEasing )
					.setDelay( outDelay ).id );
			} else
			{
				var xy = GetXY( );
				Vector3 newPos = startPos
					+ new Vector3( xy.x, xy.y, 0 );
				if ( resetOnStart )
					moveObject.transform.localPosition = startPos;
				if ( insta )
				{
					moveObject.transform.localPosition = newPos;
					return;
				}
				animIds.Add( LeanTween.moveLocal( moveObject, newPos, animationTime )
					.setEase( dissapearEasing )
					.setDelay( outDelay ).id );
			}
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( !isLocal )
			{
				if ( resetOnStart )
				{
					var xy = GetXY( );
					Vector3 newPos = startPos
						+ new Vector3( xy.x, xy.y, 0 );
					moveObject.transform.position = newPos;
				}
				if ( insta )
				{
					moveObject.transform.position = startPos;
					return;
				}

				animIds.Add( LeanTween.move( moveObject, startPos, animationTime )
					.setEase( appearEasing )
					.setDelay( inDelay ).id );
			}
			else
			{
				if ( resetOnStart )
				{
					var xy = GetXY( );
					Vector3 newPos = startPos
						+ new Vector3( xy.x, xy.y, 0 );
					moveObject.transform.localPosition = newPos;
				}
				if ( insta )
				{
					moveObject.transform.localPosition = startPos;
					return;
				}
				animIds.Add( LeanTween.moveLocal( moveObject, startPos, animationTime )
					.setEase( appearEasing )
					.setDelay( inDelay ).id );
			}
		}

		private Vector2 GetXY( )
		{
			float x = moveType.Equals( EMoveType.Percent ) ?
				Screen.width * xOffset :
				xOffsetUnits;
			float y = moveType.Equals( EMoveType.Percent ) ?
				Screen.height * -yOffset :
				-yOffsetUnits;
			return new Vector2( x, y );
		}
	}

	public enum EMoveType
	{
		Units,
		Percent
	}
}