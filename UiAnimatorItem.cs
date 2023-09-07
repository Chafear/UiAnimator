using System;
using UnityEngine;

namespace UiAnimation
{
	[Serializable]
	public class UiAnimatorItem
	{
		[SerializeField] public bool foldout = true;
		[SerializeField] public AlphaAnimation alphaAnimator = new AlphaAnimation( );
		[SerializeField] public MoveAnimation moveAnimator = new MoveAnimation( );
		[SerializeField] public ScaleAnimation scaleAnimator = new ScaleAnimation( );
		[SerializeField] public SizeAnimation sizeAnimator = new SizeAnimation( );
		[SerializeField] public EAnimationType selected;

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