using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class MonsterObject : IGameObject
	{
		public enum Direction { Down, RightDown, RightUp, LeftUp, LeftDown };
		public Direction _direction;
		public int step;
		public int DropDown = 11;
		public static Random random = new Random();
		public Canvas _canvas;
		public MonsterObject(Canvas canvas, int X, int Y, int level)
		{
			TopLeftPosistionX = X;
			TopLeftPosistionY = Y;
			Width = 20;
			Height = 20;
			IsAlive = true;
			step = 2;
			var directionAsNumber = random.Next(1, 5);
			_direction = (Direction)directionAsNumber;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/monster.png", Width, Height);
			GameObjectHelper.AddToGameBoard(Image, TopLeftPosistionY, TopLeftPosistionX, canvas);
			_canvas = canvas;
			if (level > 7)
				step += 1;
			else 
				DropDown -= level;
		}
		public MonsterObject(Canvas canvas, int level)
		{
			TopLeftPosistionX = random.Next(10, (int)canvas.Width);
			TopLeftPosistionY = random.Next(10, (int)canvas.Height/2);
			Width = 20;
			Height = 20;
			IsAlive = true;
			step = 2;
			if (level > 7)
				step += 1;
			else
				DropDown -= level;
			var directionAsNumber = random.Next(1, 5);
			_direction = (Direction)directionAsNumber;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/monster.png", Width, Height);
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
			if (!IsAlive)
			{
				Dispose();
			}
			else
			{
				CheckDirection();
				Move();
			}
			return null;
			}
		private void Move()
		{
			switch (_direction)
			{
				case Direction.Down:
					{
						TopLeftPosistionY += step;
					}; break;

				case Direction.RightDown:
					{
						TopLeftPosistionX += step;
						TopLeftPosistionY += step;
					}; break;

				case Direction.RightUp:
					{
						TopLeftPosistionX += step;
						TopLeftPosistionY -= step;
					}; break;

				case Direction.LeftUp:
					{
						TopLeftPosistionX -= step;
						TopLeftPosistionY -= step;
					}; break;

				case Direction.LeftDown:
					{
						TopLeftPosistionX -= step;
						TopLeftPosistionY += step;
					}; break;
			}
			Canvas.SetTop(Image, TopLeftPosistionY);
			Canvas.SetLeft(Image, TopLeftPosistionX);
		}

		private void CheckDirection()
		{
			if (TopLeftPosistionX < Width / 2 && _direction == Direction.LeftUp) _direction = Direction.RightUp;
			if (TopLeftPosistionX < Width / 2 && _direction == Direction.LeftDown) _direction = Direction.RightDown;
			if (TopLeftPosistionX > _canvas.Width - Width / 2 && _direction == Direction.RightDown) _direction = Direction.LeftDown;
			if (TopLeftPosistionX > _canvas.Width - Width / 2 && _direction == Direction.RightUp) _direction = Direction.LeftUp;
			if (TopLeftPosistionY > _canvas.Height / 2 && _direction == Direction.RightDown)
				if (random.Next(0, DropDown) == DropDown - 1) _direction = Direction.Down;
				else _direction = Direction.RightUp;
			if (TopLeftPosistionY > _canvas.Height / 2 && _direction == Direction.LeftDown)
				if (random.Next(0, DropDown) == DropDown - 1) _direction = Direction.Down;
				else _direction = Direction.LeftUp;
			if (TopLeftPosistionY < Width / 2 + 10 && _direction == Direction.RightUp) _direction = Direction.RightDown;
			if (TopLeftPosistionY < Width / 2 + 10 && _direction == Direction.LeftUp) _direction = Direction.LeftDown;
			if (TopLeftPosistionY > _canvas.Height - Width / 2 - 20)
			{
				TopLeftPosistionY = 0;
				var directionAsNumber = random.Next(1, 5);
				_direction = (Direction)directionAsNumber;
			}
		}
	}
}