using System.Collections.Generic;
using UnityEngine;

namespace UiAnimation
{
	public abstract class UiAnimationBase
	{
		[SerializeField] protected bool isActive = true;
		[SerializeField] protected bool autoPlay = false;
		[SerializeField] protected bool isInverse = false;
		[SerializeField] protected bool resetOnStart = false;
		[Header( "Easings" )]
		[SerializeField] protected LeanTweenType appearEasing = LeanTweenType.easeOutCubic;
		[SerializeField] protected LeanTweenType dissapearEasing = LeanTweenType.easeInCubic;
		[Header( "Time Settings" )]
		[SerializeField] protected float inDelay = 0f;
		[SerializeField] protected float outDelay = 0;
		[SerializeField] protected float animationTime = .2f;

		protected bool isInited = false;

		protected List<int> animIds = new List<int>( );

		public float AnimationTime => animationTime;
		public float InDelay => inDelay;
		public float OutDelay => outDelay;

		public abstract void OnInit( );

		public abstract void InPlay( bool insta = false );

		public abstract void OutPlay( bool insta = false );

		public void Init( )
		{
			if ( !isActive ) return;
			OnInit( );
		}

		public void In( bool insta = false )
		{
			if ( !isActive ) return;
			if ( !isInited )
			{
				Init( );
			}
			if ( isInverse )
			{
				OutPlay( insta );
			}
			else
			{
				InPlay( insta );
			}
		}

		public void Out( bool insta = false )
		{
			if ( !isActive ) return;
			if ( !isInited )
			{
				Init( );
			}
			if ( isInverse )
			{
				InPlay( insta );
			}
			else
			{
				OutPlay( insta );
			}
		}

		protected void CancelAnim( )
		{
			foreach ( var item in animIds )	LeanTween.cancel( item );
			animIds.Clear( );
		}
	}
}