using System.Collections.Generic;
using UnityEngine;

namespace UiAnimation
{
	public abstract class UiAnimatorBase
	{
		[SerializeField] public bool isActive = true;
		[SerializeField] public bool autoPlay = false;
		[SerializeField] public bool isInverse = false;
		[SerializeField] public bool resetOnStart = false;
		[Header( "Easings" )]
		[SerializeField] public LeanTweenType appearEasing = LeanTweenType.easeOutCubic;
		[SerializeField] public LeanTweenType dissapearEasing = LeanTweenType.easeInCubic;
		[Header( "Time Settings" )]
		[SerializeField] public float inDelay = 0f;
		[SerializeField] public float outDelay = 0;
		[SerializeField] public float animationTime = .2f;

		protected bool isInited = false;

		protected List<int> animIds = new List<int>( );

		public virtual float AnimationTime { get; }

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