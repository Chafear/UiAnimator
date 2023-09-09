using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class ScaleAnimation : UiAnimationBase
	{
		[SerializeField] private GameObject gameObject;
		[SerializeField] private Vector3 from = Vector3.one;
		[SerializeField] private Vector3 to = Vector3.one;

		public override void OnInit( )
		{
			if ( gameObject == null )
			{
				isActive = false;
				return;
			}
			isInited = true;
			if ( !autoPlay ) return;
			gameObject.transform.localScale = from;
		}

		public override void InPlay(bool insta = false )
		{
			CancelAnim( );
			if ( resetOnStart ) gameObject.transform.localScale = from;
			if ( insta )
			{
				gameObject.transform.localScale = to;
				return;
			}
			animIds.Add( LeanTween.scale( gameObject, to, animationTime )
				.setEase( appearEasing )
				.setDelay( inDelay ).id);

		}

		public override void OutPlay( bool insta = false )
		{
			CancelAnim( );
			if ( insta )
			{
				gameObject.transform.localScale = from;
				return;
			}

			if ( resetOnStart ) gameObject.transform.localScale = to;

			gameObject.transform.localScale = to;
			animIds.Add( LeanTween.scale( gameObject, from, animationTime )
				.setEase( dissapearEasing )
				.setDelay( outDelay ).id);
		}
	}
}