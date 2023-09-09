using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class UiAnimatorItem
	{
		[SerializeField] private bool foldout = true;

		[SerializeField] private AlphaAnimation alphaAnimation = new AlphaAnimation( );
		[SerializeField] private MoveAnimation moveAnimation = new MoveAnimation( );
		[SerializeField] private ScaleAnimation scaleAnimation = new ScaleAnimation( );
		[SerializeField] private SizeAnimation sizeAnimation = new SizeAnimation( );

		[SerializeField] private EAnimationType selected;

		private UiAnimationBase selectedAnimation;

		public UiAnimationBase SelectedAnimation => selectedAnimation;

		public void Init( )
		{
			switch ( selected )
			{
				case EAnimationType.Move: selectedAnimation = moveAnimation; break;
				case EAnimationType.Alpha: selectedAnimation = alphaAnimation; break;
				case EAnimationType.Scale: selectedAnimation = scaleAnimation; break;
				case EAnimationType.Size: selectedAnimation = sizeAnimation; break;
			}
			selectedAnimation.Init( );
		}

		public void In( bool insta = false )
		{
			selectedAnimation.In( insta );
		}

		public void Out( bool insta = false )
		{
			selectedAnimation.Out( insta );
		}
	}
}