using System;
using System.Collections.Generic;
using Windows.Media.Playback;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class ExplosionObject : IGameObject
	{
		public int TimeTillExplode { get; set; } = 20;
		public MediaPlayer boom;
		public Canvas _canvas;
		public ExplosionObject(int topLeftX, int topLeftY, Canvas canvas)
		{
			boom = new MediaPlayer { Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(@"ms-appx:///Assets/boom.mp3", UriKind.RelativeOrAbsolute)) };
			boom.Play();
			TopLeftPosistionX = topLeftX;
			TopLeftPosistionY = topLeftY;
			Width = 20;
			Height = 20;
			IsAlive = true;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/bullet.png", Width, Height);
			GameObjectHelper.AddToGameBoard(Image, TopLeftPosistionY, TopLeftPosistionX, canvas);
			_canvas = canvas;
		}
		public override void Dispose()
		{
			boom.Dispose();
			_canvas.Children.Remove(Image);
			Image = null;
		}

		public override ExplosionObject Recalculate(List<IGameObject> _gameObjects, IGameObject go, bool leftKeyPressed, bool rightKeyPressed, ref int _score)
		{
			TimeTillExplode--;
			if (TimeTillExplode == 0)
			{
				
				IsAlive = false;
				Dispose();	
			}
			return null;
		}
	}
}
