using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class AlphaAnimation : UiAnimationBase
	{
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private bool autoDisableCanvasGroup = false;
		[Range( 0f, 1.0f )]
		[SerializeField] private float from = 0;
		[Range( 0f, 1.0f )]
		[SerializeField] private float to = 1;

		public override void OnInit( )
		{
			if ( canvasGroup == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			canvasGroup.alpha = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) canvasGroup.alpha = from;
			if( insta )
			{
				canvasGroup.alpha = to;
				return;
			}
			animIds.Add( LeanTween.alphaCanvas( canvasGroup, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay )
				.setOnComplete( ( ) =>
				{
					if ( autoDisableCanvasGroup )
					{
						canvasGroup.blocksRaycasts = true;
						canvasGroup.interactable = true;
					}
				} )
				.id );
		}

		public override void OutPlay( bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) canvasGroup.alpha = to;
			if ( insta )
			{
				canvasGroup.alpha = from;
				if ( autoDisableCanvasGroup )
				{
					canvasGroup.blocksRaycasts = false;
					canvasGroup.interactable = false;
				}
				return;
			}
			animIds.Add( LeanTween.alphaCanvas( canvasGroup, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay )
				.setOnComplete( ( ) =>
				{
					if ( autoDisableCanvasGroup )
					{
						canvasGroup.blocksRaycasts = false;
						canvasGroup.interactable = false;
					}
				} )
				.id );
		}
	}
}


