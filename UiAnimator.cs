using System;
using System.Collections.Generic;
using UnityEngine;

namespace UiAnimation
{
	public class UiAnimator : MonoBehaviour
	{
		[SerializeField] public bool AllowInit = true;
		[SerializeField] private List<UiAnimatorItem> items = new List<UiAnimatorItem>( );

		private float playInTime;
		private float playOutTime;

		private event Action onFinishInCallback;
		private event Action onFinishOutCallback;

		private int callbackInId;
		private int callbackOutId;

		public List<UiAnimatorItem> Items => items;

		private void Awake( )
		{
			if ( AllowInit ) Init( );
			foreach ( var item in items )
			{
				var animation = item.SelectedAnimation;
				var timeIn = animation.AnimationTime + animation.inDelay;
				playInTime = playInTime > timeIn ? playInTime : timeIn;
				var timeOut = animation.AnimationTime + animation.inDelay;
				playOutTime = playOutTime > timeOut ? playOutTime : timeOut;
			}
		}

		public void SetOnFinishInCallback( Action onFinishInCallback )
		{
			this.onFinishInCallback = onFinishInCallback;
		}
		
		public void SetOnFinishOutCallback( Action onFinishOutCallback )
		{
			this.onFinishOutCallback = onFinishOutCallback;
		}

		public void Add( )
		{
			items.Add( new UiAnimatorItem( ) );
		}

		public void RemoveAt( int index )
		{
			items.RemoveAt( index );
		}

		public void Down( int index )
		{
			if(items.Count - 1 == index ) return;
			var tempA = items[ index ];
			var tempB = items[index+1];
			items[ index ] = tempB;
			items[ index+1 ] = tempA;
		}

		public void Up( int index )
		{
			if ( index == 0 ) return;
			var tempA = items[index];
			var tempB = items[index - 1];
			items[index] = tempB;
			items[index - 1] = tempA;
		}

		public void In( )
		{
			foreach ( var item in items ) { item.In( ); }
			if ( onFinishInCallback == null ) return;
			LeanTween.cancel( callbackInId );
			callbackInId = LeanTween.delayedCall( playInTime, onFinishInCallback ).id;
		}

		public void Out( )
		{
			foreach ( var item in items ) { item.Out( ); }
			if ( onFinishInCallback == null ) return;
			LeanTween.cancel( callbackOutId );
			callbackOutId = LeanTween.delayedCall( playInTime, onFinishOutCallback ).id;
		}

		public void Init( )
		{
			foreach ( var item in items ) { item.Init( ); }
		}

		public void In( bool insta )
		{
			foreach ( var item in items ) { item.In( insta ); }
		}

		public void Out( bool insta )
		{
			foreach ( var item in items ) { item.Out( insta ); }
		}
	}
}
