using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class AlphaAnimation : UiAnimationBase
	{
		[SerializeField] private CanvasGroup cg;
		[SerializeField] private bool autoOff = false;
		[Range( 0f, 1.0f )]
		[SerializeField] private float from = 0;
		[Range( 0f, 1.0f )]
		[SerializeField] private float to = 1;

		public override void OnInit( )
		{
			if ( cg == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			cg.alpha = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) cg.alpha = from;
			if( insta )
			{
				cg.alpha = to;
				return;
			}
			animIds.Add( LeanTween.alphaCanvas( cg, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay )
				.setOnComplete( ( ) =>
				{
					if ( autoOff )
					{
						cg.blocksRaycasts = true;
						cg.interactable = true;
					}
				} )
				.id );
		}

		public override void OutPlay( bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) cg.alpha = to;
			if ( insta )
			{
				cg.alpha = from;
				if ( autoOff )
				{
					cg.blocksRaycasts = false;
					cg.interactable = false;
				}
				return;
			}
			animIds.Add( LeanTween.alphaCanvas( cg, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay )
				.setOnComplete( ( ) =>
				{
					if ( autoOff )
					{
						cg.blocksRaycasts = false;
						cg.interactable = false;
					}
				} )
				.id );
		}
	}
}


