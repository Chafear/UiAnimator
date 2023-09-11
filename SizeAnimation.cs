using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public sealed class SizeAnimation : UiAnimationBase
	{
		[SerializeField] private RectTransform toResize;
		[SerializeField] private Vector2 from = Vector2.zero;
		[SerializeField] private Vector2 to = Vector2.zero;

		public override void OnInit( )
		{
			if ( toResize == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			toResize.sizeDelta = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) toResize.sizeDelta = from;
			if( insta )
			{
				toResize.sizeDelta = to;
				return;
			}
			animIds.Add( LeanTween.size( toResize, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay ).id );
		}

		public override void OutPlay( bool insta = false )
		{
			if ( !isActive ) return;

			CancelAnim( );
			if ( resetOnStart ) toResize.sizeDelta = to;
			if ( insta )
			{
				toResize.sizeDelta = from;
				return;
			}
			animIds.Add( LeanTween.size( toResize, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay ).id );
		}

		public void ResetToFrom( )
		{
			toResize.sizeDelta = from;
		}
	}
}