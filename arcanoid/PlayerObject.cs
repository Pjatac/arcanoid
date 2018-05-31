using System.Collections.Generic;
using System.Xml.Serialization;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class PlayerObject : IGameObject
	{
		public enum Direction { Left, Right, Stay };
		public Direction _direction;
		public int step;
		public int lives;
		public Canvas _canvas;

		public PlayerObject(int topLeftX, int topLeftY, Canvas canvas)
		{
			_canvas = canvas;
			TopLeftPosistionX = topLeftX;
			TopLeftPosistionY = topLeftY;
			Width = 43;
			Height = 9;
			step = 5;
			IsAlive = true;
			lives = 3;
			_direction = Direction.Stay;
			Image = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/arkanoid.png", Width, Height);
			GameObjectHelper.AddToGameBoard(Image, TopLeftPosistionY, TopLeftPosistionX, canvas);
		}
		public override void Dispose()
		{
			_canvas.Children.Remove(Image);
			Image = null;
		}
		public override ExplosionObject Recalculate(List<IGameObject> io, IGameObject go, bool left, bool right, ref int _score)
		{
			foreach (var monster in io)
			{
				if (monster is MonsterObject && monster.TopLeftPosistionY + monster.Width / 2 > TopLeftPosistionY && monster.TopLeftPosistionX >= TopLeftPosistionX && (monster.TopLeftPosistionX - TopLeftPosistionX) < 43 && (monster as MonsterObject)._direction == 0)
				{
					monster.IsAlive = false;
					lives -= 1;
					if (lives == 0) IsAlive = false;
					return new ExplosionObject(TopLeftPosistionX + Width / 2 - 10, TopLeftPosistionY - 10, _canvas);
				}
			}
			CheckDirection(left, right);
			Move();
			return null;
		}
		private void CheckDirection(bool left, bool right)
		{
			if (right && TopLeftPosistionX < _canvas.Width - 43)
			{
				_direction = PlayerObject.Direction.Right;
			}
			else if (left && TopLeftPosistionX > 22)
			{
				_direction = PlayerObject.Direction.Left;
			}
			else
			{
				_direction = PlayerObject.Direction.Stay;
			}
		}

		private void Move()
		{
			switch (_direction)
			{
				case Direction.Left:
					{
						TopLeftPosistionX -= step;
					}; break;

				case Direction.Right:
					{
						TopLeftPosistionX += step;
					}; break;
			}
			Canvas.SetLeft(Image, TopLeftPosistionX);
		}
	}
}