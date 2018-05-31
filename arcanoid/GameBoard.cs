using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class GameBoard : IDisposable
	{
		public int _iteration = 0;
		public int _score = 0;
		public int _lives;
		public int _level = 1;

		public Canvas _canvas;
		public PlayerObject _player;
		public List<IGameObject> _gameObjects = new List<IGameObject>();
		public static Random random = new Random();

		public int MaxMonsterCount = 40;
		public int MaxBulletCount = 100;
		public int CreateMonstersEveryNthIteration = 10;

		public GameBoard(Canvas canvas)
		{
			_canvas = canvas;
			Image Sky = GameObjectHelper.ReadImageFromFile(@"ms-appx:///Assets/2480x1200.png", 820, 470);
			GameObjectHelper.AddToGameBoard(Sky, -10, -10, canvas);
			_player = new PlayerObject((int)_canvas.Width / 2 - 21, (int)_canvas.Height - 40, _canvas);
			_player.IsAlive = true;
			_gameObjects.Add(_player);		
		}

		public void LevelUp()
		{
			MaxMonsterCount += 10;
			if (MaxBulletCount != 10)
				MaxBulletCount -= 10;
			if (CreateMonstersEveryNthIteration != 3)
				CreateMonstersEveryNthIteration -= 1;
			_level += 1;
		}

		public bool Recalculate(bool leftKeyPressed, bool rightKeyPressed, bool spaceKeyPressed)
		{
			Add(spaceKeyPressed); // Check and add bullet & monster

			List<ExplosionObject> toAdd = new List<ExplosionObject>(); // Create empty list of explosion's, that 'll need to add

			foreach (var go in _gameObjects)
			{
				toAdd.Add(go.Recalculate(_gameObjects, go, leftKeyPressed, rightKeyPressed, ref _score));   // Recalculate all of IGameObject's
			}                                                                                               // and form list of explosions

			foreach (var exp in toAdd)
			{
				if (exp != null)
				{
					_gameObjects.Add(exp); // Add explosions to GameBoard
				}
			}

			_gameObjects.RemoveAll(gameObject => !gameObject.IsAlive); // Remove all died objects from GameBoard
			_lives = _player.lives;
			_iteration++;

			return _player.IsAlive;
		}
		private void Add(bool spaceKeyPressed)  // Check and add bullet & monster
		{
			int mCount = _gameObjects.Count(gameObject => gameObject is MonsterObject);
			if (_iteration % CreateMonstersEveryNthIteration == 0 && mCount < MaxMonsterCount)
			{
				var monster = new MonsterObject(_canvas, _level);
				_gameObjects.Add(monster);
			}
			int bCount = _gameObjects.Count(gameObject => gameObject is BulletObject);
			if (spaceKeyPressed && bCount < MaxBulletCount)
			{
				var bullet = new BulletObject(_player.TopLeftPosistionX + 19, _canvas);
				_gameObjects.Add(bullet);
			}
		}

		public void Dispose()
		{
		}
	}
}
