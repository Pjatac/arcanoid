using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace arcanoid
{
	public class IGameObject : IDisposable
	{
		public int TopLeftPosistionX { get; set; }
		public int TopLeftPosistionY { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public bool IsAlive { get; set; }
		public Image Image { get; set; }
		public virtual ExplosionObject Recalculate(List<IGameObject> _gameObjects, IGameObject go, bool leftKeyPressed, bool rightKeyPressed, ref int _score)
		{
			return null;
		}
		public virtual void Dispose()
		{
		}
	}
}
