using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class SizeAnimation : UiAnimationBase
	{
		[SerializeField] private RectTransform rect;
		[SerializeField] private Vector2 from = Vector2.zero;
		[SerializeField] private Vector2 to = Vector2.zero;

		public override void OnInit( )
		{
			if ( rect == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			rect.sizeDelta = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) rect.sizeDelta = from;
			if( insta )
			{
				rect.sizeDelta = to;
				return;
			}
			animIds.Add( LeanTween.size( rect, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay ).id );
		}

		public override void OutPlay( bool insta = false )
		{
			if ( !isActive ) return;

			CancelAnim( );
			if ( resetOnStart ) rect.sizeDelta = to;
			if ( insta )
			{
				rect.sizeDelta = from;
				return;
			}
			animIds.Add( LeanTween.size( rect, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay ).id );
		}

		public void ResetToFrom( )
		{
			rect.sizeDelta = from;
		}
	}
}