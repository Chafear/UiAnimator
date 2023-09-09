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

		private UiAnimatorBase selectedAnimation;

		public UiAnimatorBase SelectedAnimation => selectedAnimation;

		public void Init( )
		{
			switch ( selected )
			{
				case EAnimationType.Move: selectedAnimation = moveAnimator;	break;
				case EAnimationType.Alpha: selectedAnimation = alphaAnimator; break;
				case EAnimationType.Scale: selectedAnimation = scaleAnimator; break;
				case EAnimationType.Size: selectedAnimation = sizeAnimator; break;
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