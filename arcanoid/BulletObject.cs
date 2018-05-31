using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class BulletObject : IGameObject
	{
		public int step;
		public Canvas _canvas;
		public BulletObject(Canvas canvas, int X, int Y)
		{
			TopLeftPosistionX = X;
			TopLeftPosistionY = Y;
			Width = 5;
			Height = 5;
			step = 3;
			IsAlive = true;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/Bullet.png", Width, Height);
			GameObjectHelper.AddToGameBoard(Image, TopLeftPosistionY, TopLeftPosistionX, canvas);
			_canvas = canvas;
		}
		public BulletObject(int topLeftX, Canvas canvas)
		{
			TopLeftPosistionX = topLeftX;
			TopLeftPosistionY = (int)canvas.Height - 60;
			Width = 5;
			Height = 5;
			step = 3;
			IsAlive = true;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/Bullet.png", Width, Height);
			GameObjectHelper.AddToGameBoard(Image, TopLeftPosistionY, TopLeftPosistionX, canvas);
			_canvas = canvas;
		}
		public override void Dispose()
		{
			_canvas.Children.Remove(Image);
			Image = null;
		}
		public override ExplosionObject Recalculate(List<IGameObject> io, IGameObject go, bool leftKeyPressed, bool rightKeyPressed, ref int _score)
		{
			foreach (var monster in io)
			{
				if (monster is MonsterObject && TopLeftPosistionY == (monster.TopLeftPosistionY + monster.Width) && TopLeftPosistionX >= monster.TopLeftPosistionX && (TopLeftPosistionX - monster.TopLeftPosistionX) < 16)
				{
					if ((monster as MonsterObject)._direction == 0) _score += 3; else _score += 1;
					IsAlive = false;
					monster.IsAlive = false;
					monster.Dispose();
					Dispose();
					return new ExplosionObject(TopLeftPosistionX, TopLeftPosistionY - 10, _canvas);
				}
			}
			TopLeftPosistionY -= step;
			Canvas.SetTop(Image, TopLeftPosistionY);
			if (TopLeftPosistionY < 3)
			{
				IsAlive = false;
				Dispose();
			}
			return null;
		}
	}
}
