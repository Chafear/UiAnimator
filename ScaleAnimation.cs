using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public sealed class ScaleAnimation : UiAnimationBase
	{
		[SerializeField] private GameObject scaleObject;
		[SerializeField] private Vector3 from = Vector3.one;
		[SerializeField] private Vector3 to = Vector3.one;

		public override void OnInit( )
		{
			if ( scaleObject == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			scaleObject.transform.localScale = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) scaleObject.transform.localScale = from;
			if ( insta )
			{
				scaleObject.transform.localScale = to;
				return;
			}
			animIds.Add( LeanTween.scale( scaleObject, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay ).id);

		}

		public override void OutPlay( bool insta = false )
		{
			CancelAnim( );
			if ( insta )
			{
				scaleObject.transform.localScale = from;
				return;
			}

			if ( resetOnStart ) scaleObject.transform.localScale = to;

			scaleObject.transform.localScale = to;
			animIds.Add( LeanTween.scale( scaleObject, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay ).id);
		}
	}
}